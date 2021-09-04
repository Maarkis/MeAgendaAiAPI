using System;
using System.Threading.Tasks;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MeAgendaAi.CrossCutting.DependencyInjection
{
    public class ConfigureJwt
    {
        public static void ConfigureDependenciesJwt(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var signingConfiguration = new SigningConfiguration();
            serviceCollection.AddSingleton(signingConfiguration);

            var tokenConfiguration = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(
                configuration.GetSection("TokenConfiguration")).Configure(tokenConfiguration);
            serviceCollection.AddSingleton(tokenConfiguration);


            serviceCollection.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                bearerOptions.RequireHttpsMetadata = false;
                bearerOptions.SaveToken = true;

                bearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = signingConfiguration.Key,
                    ValidateAudience = true,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidateIssuer = true,
                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                bearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired", "true");
                        return Task.CompletedTask;
                    }
                };
            });


            serviceCollection.AddAuthorization(auth =>
            {
                auth.AddPolicy("Administrador", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireRole(Roles.Admin.ToString())
                    .Build());

                auth.AddPolicy("Cliente", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireRole(Roles.Cliente.ToString())
                    .Build());

                auth.AddPolicy("Funcionario", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireRole(Roles.Funcionario.ToString())
                    .Build());

                auth.AddPolicy("UsuarioEmpresa", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .RequireRole(Roles.UsuarioEmpresa.ToString())
                    .Build());
            });
        }
    }
}