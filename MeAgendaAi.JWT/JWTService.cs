using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MeAgendaAi.JWT
{
    public class JWTService
    {
        private static SigningConfiguration _signingConfiguration;
        private static TokenConfiguration _tokenConfiguration;

        public static ResponseAuthentication GenerateToken(User user, SigningConfiguration signingConfiguration, TokenConfiguration tokenConfiguration)
        {

            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;

            List<Claim> claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
            };

            user.Roles.ForEach(role =>
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Role.ToString()));
            });

            ClaimsIdentity identity = new ClaimsIdentity(
              new GenericIdentity(user.Email),
              claims);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate + TimeSpan.FromSeconds(Convert.ToDouble(_tokenConfiguration.Seconds));

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = CreateToken(identity, createDate, expirationDate, handler);

            
            return SuccessObject(createDate, expirationDate, token, user);
        }

        public static string GenerateTokenRecoverPassword(User user, SigningConfiguration signingConfiguration, TokenConfiguration tokenConfiguration)
        {

            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;


            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate + TimeSpan.FromSeconds(Convert.ToDouble(_tokenConfiguration.Seconds));

            List<Claim> claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),                    
                    new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),                    
                    new Claim(ClaimTypes.Expiration, expirationDate.ToString()),
            };


            ClaimsIdentity identity = new ClaimsIdentity(
              new GenericIdentity(user.Email),
              claims);


            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return CreateToken(identity, createDate, expirationDate, handler);
            
        }

        private static ResponseAuthentication SuccessObject(DateTime createDate, DateTime expirationDate, string token, User user)
        {
            int role = 0;
            if(user.Roles.Any(x => x.Role == Domain.Enums.Roles.Admin))
            {
                role = (int)Domain.Enums.Roles.Admin;
            }
            else
            {
                role = (int)user.Roles.FirstOrDefault().Role;
            }

            return new ResponseAuthentication()
            {
                Authenticated = true,
                Create = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Token = token,
                Id = user.UserId,
                UserName = user.Name,
                UserEmail = user.Email,                
                Message = "Usuário autenticado com sucesso",
                Role = role
            };
        }

        private static string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            SecurityToken securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate,
                
            });                        
            
            return handler.WriteToken(securityToken);
        }

        public static bool ValidateToken(string token, SigningConfiguration signingConfiguration, TokenConfiguration tokenConfiguration)
        {

            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParameters = GetValidationParameters();

            SecurityToken validatedToken;            
            try
            {
                ClaimsPrincipal resp = handler.ValidateToken(token, validationParameters, out validatedToken);                
                
            }
            catch (Exception e)
            {
                return false;
            }
            
            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = _tokenConfiguration.Issuer,
                ValidAudience = _tokenConfiguration.Audience,
                IssuerSigningKey = _signingConfiguration.Key
            };
        }

    }
}
