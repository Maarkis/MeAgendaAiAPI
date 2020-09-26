using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.AddClient;
using MeAgendaAi.Service.Validators.AddUser;
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

        public bool ValidateUser(User user)
        {
            bool resp = false;
            var userVal = new AddUserModelValidator().Validate(user);
            if (userVal.IsValid)
            {
                resp = true;
            }

            return resp;
        }
    }
}
