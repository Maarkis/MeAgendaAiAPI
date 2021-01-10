using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.User;
using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Enums;

namespace MeAgendaAi.Service.Services
{
    public class ClientService : BaseService<Client>, IClientService
    {
        private IClientRepository _clientRepository;
        private IUserService _userService;

        public ClientService(IClientRepository clientRepository,
            IUserService userService) : base(clientRepository)
        {
            _clientRepository = clientRepository;
            _userService = userService;
        }

        public ResponseModel AddClient(AddUserModel model)
        {
            var resp = new ResponseModel();

            try
            {
                List<Roles> roles = new List<Roles>();
                roles.Add(Roles.Cliente);
                var userResponse = _userService.CreateUserFromModel(model, roles);
                if (userResponse.Success)
                {
                    User newUser = userResponse.Result as User;
                    Client newClient = new Client
                    {
                        ClientId = Guid.NewGuid(),
                        UserId = newUser.UserId,
                        CreatedAt = DateTimeUtil.UtcToBrasilia(),
                        LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                        User = newUser
                    };
                    _clientRepository.Add(newClient);

                    resp.Success = true;
                    resp.Result = "Cliente adicionado com sucesso";
                }
                else
                {
                    resp = userResponse;
                }

                return resp;
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar o cliente";
            }

            return resp;
        }
    }
}
