using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.User;
using MeAgendaAi.Service.Validators.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Services
{
    public class UserService : BaseService<User>, IUserService
    {
        private IUserRepository _userRepository;
        private IClientRepository _clientRepository;
        public UserService(IUserRepository userRepository, IClientRepository clientRepository) : base(userRepository)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
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
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
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
    }
}
