
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Security;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.User;
using MeAgendaAi.Service.Validators.User;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;

namespace MeAgendaAi.Service.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private IUserRepository _userRepository;
        private IClientRepository _clientRepository;

        private readonly SigningConfiguration _signingConfiguration;
        private readonly TokenConfiguration _tokenConfiguration;

        public UserService(
            IUserRepository userRepository,
            IClientRepository clientRepository, 
            SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration) : base(userRepository)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;
        }

        public bool ValidateUser(AddUserModel user)
        {
            bool resp = false;
            var userVal = new AddUserModelValidator().Validate(user);
            if (userVal.IsValid)
            {
                resp = true;
            }

            return resp;
        }

        public ResponseModel AddUser(AddUserModel model)
        {
            var resp = new ResponseModel();

            try
            {
                User newUser = new User
                {
                    UserId = Guid.NewGuid(),
                    Email = model.Email,
                    Password = model.Password,
                    Name = model.Name,
                    CPF = model.CPF,
                    RG = model.RG,
                    CreatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia()
                };
                _userRepository.Add(newUser);

                resp.Success = true;
                resp.Result = "Usuário adicionado com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar o usuário";
            }

            return resp;
        }

        public ResponseModel LoginMock()
        {
            var resp = new ResponseModel();

            try
            {
                Guid id = Guid.Parse("D0605249-9E36-4551-A01F-C7D5D52B9A58");
                User user = _userRepository.GetById(id);


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


                resp.Success = true;
                resp.Result = SuccessObject(createDate, expirationDate, token, user);
                //return SuccessObject(createDate, expirationDate, token, user);

            }
            catch (Exception)
            {
                resp.Result = "Não foi possível encontrar o usuário";
            }

            return resp;
        }

         private string CreateToken(ClaimsIdentity identity, DateTime createDate, DateTime expirationDate, JwtSecurityTokenHandler handler)
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

            string token = handler.WriteToken(securityToken);
            return token;
        }

        private object SuccessObject(DateTime createDate, DateTime expirationDate, string token, User user)
        {
            return new
            {
                authenticated = true,
                create = createDate.ToString("yyyy-MM-dd HH:mm:ss"),
                expiration = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                token = token,
                userName = user.Name,
                userEmail = user.Email,
                message = "Usuário autenticado com sucesso"
            };

        }
    }
}
