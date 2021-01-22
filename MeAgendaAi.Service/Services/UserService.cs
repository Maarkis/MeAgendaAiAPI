using FluentValidation.Results;
using MeAgendaAi.Cryptography.Cryptography;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Interfaces.Services.Email;
using MeAgendaAi.Domain.Security;
using MeAgendaAi.Domain.Validators.Authentication;
using MeAgendaAi.Domain.Validators.User;
using MeAgendaAi.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeAgendaAi.Service.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private IUserRepository _userRepository;
        private IClientRepository _clientRepository;
        private ILocationService _locationService;
        private readonly IEmailService _email;

        private readonly SigningConfiguration _signingConfiguration;
        private readonly TokenConfiguration _tokenConfiguration;

        public UserService(
            IEmailService email,
            IUserRepository userRepository,
            IClientRepository clientRepository,
            SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration,
            ILocationService locationService) : base(userRepository)
        {
            _email = email;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _signingConfiguration = signingConfiguration;
            _tokenConfiguration = tokenConfiguration;
            _locationService = locationService;
        }

        public ResponseModel CreateUserFromModel(AddUserModel model, List<Roles> roles)
        {
            var resp = new ResponseModel();

            try
            {
                // validate user
                var validateUser = new AddUserModelValidator().Validate(model);
                if (validateUser.IsValid)
                {

                    if(model.Locations != null && model.Locations.Count > 0)
                    {
                        var validLocations = _locationService.ValidateAddLocations(model.Locations);
                        if (!validLocations.Success)
                        {
                            return validLocations;
                        }
                    }

                    Guid userId = Guid.NewGuid();

                    User newUser = new User
                    {
                        UserId = userId,
                        Email = model.Email,
                        Password = Encrypt.EncryptString(model.Password, userId.ToString()),
                        Name = model.Name,
                        CPF = model.CPF,
                        RG = model.RG,
                        Locations = _locationService.CreateLocationsFromModel(model.Locations, userId, null),
                        CreatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                        LastUpdatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                        Roles = GenerateUserRoles(roles, userId)
                    };
                    //_userRepository.Add(newUser);

                    resp.Success = true;
                    resp.Result = newUser;
                }
                else
                {
                    resp.Result = validateUser.Errors.FirstOrDefault().ToString();
                }

                return resp;
            }
            catch (Exception e)
            {
                resp.Result = "Não foi possível adicionar o usuário";
            }

            return resp;
        }

        private List<UserRole> GenerateUserRoles(List<Roles> roles, Guid userId)
        {
            List<UserRole> userRoles = new List<UserRole>();
            roles.ForEach(role => {
                var userRole = new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId = userId,
                    Role = role,
                    CreatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = Domain.Utils.DateTimeUtil.UtcToBrasilia(),
                    UpdatedBy = userId
                };

                userRoles.Add(userRole);
            });

            return userRoles;
        }
        
        public ResponseModel Login(LoginModel model)
        {


            ResponseModel resp = new ResponseModel();

            ValidationResult validateLogin = new AuthenticationModelValidator().Validate(model);
            if (validateLogin.IsValid)
            {
                try
                {
                    User user = _userRepository.GetByEmail(model.Email);
                    if(user != null)
                    {

                        if (!ValidatePassword(model.Senha, user))
                        {
                            resp.Success = false;
                            resp.Result = "Senha inválida";

                            return resp;
                        }

                        resp.Success = true;
                        resp.Result = JWTService.GenerateToken(user, _signingConfiguration, _tokenConfiguration);
                    }
                    else
                    {
                        resp.Result = "Usuário não cadastrado";
                    }

                }
                catch (Exception)
                {
                    resp.Result = "Não foi possível encontrar o usuário";
                }
            }
            else
            {
                resp.Result = validateLogin.Errors.FirstOrDefault().ErrorMessage;
            }
            return resp;
        }


        public async Task<ResponseModel> RetrievePassword(RecoveryPassword model)
        {
            ResponseModel response = new ResponseModel();
            ValidationResult validateRecoveryEmail = new RecoveryEmailValidator().Validate(model);
            if (validateRecoveryEmail.IsValid)
            {
                try
                {
                    User user = _userRepository.GetByEmail(model.Email);
                    if (user == null)
                    {
                        response.Result = "Usuário não encontrado";

                        return response;
                    }

                    string token = JWTService.GenerateTokenRecoverPassword(user, _signingConfiguration, _tokenConfiguration);

                    bool resp = await _email.SendRecoveryPassword(user, token);
                    if (resp)
                    {
                        response.Success = resp;
                        response.Result = "E-mail enviado com sucesso.";
                    }
                    else
                    {
                        response.Result = "Não foi possível enviar o e-mail.";
                    }
                }
                catch (Exception)
                {
                    response.Result = "Erro no sistema, e-mail não enviado.";
                }
            } else
            {
                response.Result = validateRecoveryEmail.Errors.FirstOrDefault().ErrorMessage;
            }

            return response;
        }

        private bool ValidatePassword(string password, User user)
        {
            return Encrypt.CompareComputeHash(password, user.UserId.ToString(), user.Password);
        }

        public ResponseModel ResetPassword(ResetPassword model)
        {
            ResponseModel response = new ResponseModel();
            ValidationResult validateResetPassword = new ResetPasswordValidator().Validate(model);
            if (validateResetPassword.IsValid)
            {
                try
                {
                    bool validateToken = JWTService.ValidateToken(model.Token, _signingConfiguration, _tokenConfiguration);
                    if (validateToken)
                    {
                        User user = _userRepository.GetById(model.Id);
                        if(user == null)
                        {                            
                            response.Result = "Usuário não encontrado.";
                            return response;
                        }
                                                
                        user.Password = Encrypt.EncryptString(model.Password, user.UserId.ToString());
                        user.UpdatedBy = model.Id;
                        user.LastUpdatedAt = DateTime.Now;

                        _userRepository.Edit(user);


                        response.Success = true;
                        response.Result = "Senha alterada com sucesso.";
                    }
                    else
                    {
                        response.Result = "Token expirado.";
                    }
                }
                catch (Exception)
                {
                    response.Result = "Erro ao alterar senha, entre em contato com suporte.";
                }
            }
            else
            {
                response.Result = validateResetPassword.Errors.FirstOrDefault().ErrorMessage;
            }
            return response;
                
        }
    }
}
