using FluentValidation.Results;
using MeAgendaAi.Cryptography.Cryptography;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Security;
using MeAgendaAi.Domain.Validators.User;
using MeAgendaAi.JWT;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.User;
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
            ValidationResult userVal = new AddUserModelValidator().Validate(user);
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
                Guid userId = Guid.NewGuid();
                User newUser = new User
                {
                    UserId = userId,
                    Email = model.Email,
                    Password = Encrypt.EncryptString(model.Password, userId.ToString()),
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
            catch (Exception e)
            {
                resp.Result = "Não foi possível adicionar o usuário";
            }

            return resp;
        }

        public ResponseModel Login(LoginModel model)
        {
            var resp = new ResponseModel();

            try
            {
                User user = _userRepository.GetByEmail(model.Email);

                if(!ValidatePassword(model.Senha, user))
                {
                    resp.Success = false;
                    resp.Result = "Senha inválida";

                    return resp;
                }

                resp.Success = true;
                resp.Result = JWTService.GenerateToken(user, _signingConfiguration, _tokenConfiguration); 
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível encontrar o usuário";
            }

            return resp;
        }



        private bool ValidatePassword(string password, User user)
        {
            return Encrypt.CompareComputeHash(password, user.UserId.ToString(), user.Password);
        }
    }
}
