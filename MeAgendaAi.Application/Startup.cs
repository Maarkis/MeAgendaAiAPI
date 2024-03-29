using MeAgendaAi.CrossCutting.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace MeAgendaAi.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("ConnectionString");

            ConfigureRepository.ConfigureDependenciesService(services, connectionString);
            ConfigureService.ConfigureDependenciesService(services); 

            // Configuration JTW
            ConfigureJwt.ConfigureDependenciesJwt(services, Configuration);
            // End Configuration JTW

            // Configuration MailJet
            ConfigureMailJet.ConfigureDependecieMailJet(services, Configuration);
            // End Configuration MailJet

            services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Me agenda a� - API",
                    Description = "API para gerenciamento da aplica��o me agenda a�",
                    TermsOfService = new Uri("https://github.com/Maarkis/MeAgendaAiAPI"),
                    Contact = new OpenApiContact
                    {
                        Name = "Jean Markis - Manoela Viana - Vit�ria Sim�es",
                        Email = "jeanmarkis85@gmail.com;manoelanardy@gmail.com;vitoriasimoes_2000@outlook.com",
                        Url = new Uri("https://github.com/Maarkis/MeAgendaAiAPI"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors("MyAllowSpecificOrigins");


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Me Agenda Ai API - v1");
            });

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
