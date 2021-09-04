using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Security;
using Microsoft.IdentityModel.Tokens;

namespace MeAgendaAi.JWT
{
    public class JWTService
    {
        private static SigningConfiguration _signingConfiguration;
        private static TokenConfiguration _tokenConfiguration;

        public static ResponseAuthentication GenerateToken(User user, SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration)
        {
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            user.Roles.ForEach(role => { claims.Add(new Claim(ClaimTypes.Role, role.Role.ToString())); });

            var identity = new ClaimsIdentity(
                new GenericIdentity(user.Email),
                claims);

            var createDate = DateTime.Now;
            var expirationDate = createDate + TimeSpan.FromSeconds(Convert.ToDouble(_tokenConfiguration.Seconds));

            var handler = new JwtSecurityTokenHandler();
            var token = CreateToken(identity, createDate, expirationDate, handler);


            return SuccessObject(createDate, expirationDate, token, user);
        }

        public static string GenerateTokenRecoverPassword(User user, SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration)
        {
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;


            var createDate = DateTime.Now;
            var expirationDate = createDate + TimeSpan.FromSeconds(Convert.ToDouble(_tokenConfiguration.Seconds));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Expiration, expirationDate.ToString())
            };


            var identity = new ClaimsIdentity(
                new GenericIdentity(user.Email),
                claims);


            var handler = new JwtSecurityTokenHandler();
            return CreateToken(identity, createDate, expirationDate, handler);
        }

        private static ResponseAuthentication SuccessObject(DateTime createDate, DateTime expirationDate, string token,
            User user)
        {
            var role = 0;
            if (user.Roles.Any(x => x.Role == Roles.Admin))
                role = (int)Roles.Admin;
            else
                role = (int)user.Roles.FirstOrDefault().Role;

            return new ResponseAuthentication
            {
                Authenticated = true,
                Create = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                Token = token,
                Image = user.Image,
                Id = user.UserId,
                UserName = user.Name,
                UserEmail = user.Email,
                Message = "Usuário autenticado com sucesso",
                Role = role
            };
        }

        private static string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate,
            JwtSecurityTokenHandler handler)
        {
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _tokenConfiguration.Issuer,
                Audience = _tokenConfiguration.Audience,
                SigningCredentials = _signingConfiguration.SigningCredentials,
                Subject = identity,
                NotBefore = createDate,
                Expires = expirationDate
            });

            return handler.WriteToken(securityToken);
        }

        public static bool ValidateToken(string token, SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration)
        {
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;

            var handler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;
            try
            {
                var resp = handler.ValidateToken(token, validationParameters, out validatedToken);
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
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