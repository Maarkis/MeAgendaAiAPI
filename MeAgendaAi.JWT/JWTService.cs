using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Security;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

            ClaimsIdentity identity = new ClaimsIdentity(
              new GenericIdentity(user.Email),
              new[]{
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, "Admin")
              });

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate + TimeSpan.FromSeconds(Convert.ToDouble(_tokenConfiguration.Seconds));

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = CreateToken(identity, createDate, expirationDate, handler);

            return SuccessObject(createDate, expirationDate, token, user);
        }

        private static ResponseAuthentication SuccessObject(DateTime createDate, DateTime expirationDate, string token, User user)
        {
            return new ResponseAuthentication()
            {
                Authenticated = true,
                Create = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Token = token,
                UserName = user.Name,
                UserEmail = user.Email,
                Message = "Usuário autenticado com sucesso"
            };
        }

        private static string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
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
    }
}
