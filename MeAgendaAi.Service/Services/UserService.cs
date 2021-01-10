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
using MeAgendaAi.Domain.EpModels.Location;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Collections.Generic;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Validators.Location;
using MeAgendaAi.Domain.Enums;

namespace MeAgendaAi.Service.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private IUserRepository _userRepository;
        private IClientRepository _clientRepository;
        private ILocationService _locationService;

        private readonly SigningConfiguration _signingConfiguration;
        private readonly TokenConfiguration _tokenConfiguration;

        public UserService(
            IUserRepository userRepository,
            IClientRepository clientRepository,
            SigningConfiguration signingConfiguration,
            TokenConfiguration tokenConfiguration,
            ILocationService locationService) : base(userRepository)
        {
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
