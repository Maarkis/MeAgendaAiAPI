using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.AddClient;

namespace MeAgendaAi.Service.Services
{
    public class ClientService : BaseService<Client>, IClientService
    {
        private IClientRepository _clientRepository;

        public ClientService(IClientRepository userRepository, IClientRepository clientRepository) : base(userRepository)
        {
            _clientRepository = clientRepository;
        }

        public ResponseModel AddClient(AddClientEpModel model)
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
                    RG = model.RG
                };
                //_userRepository.Add(newUser);
                Client newClient = new Client
                {
                    ClientId = Guid.NewGuid(),
                    User = newUser
                };
                _clientRepository.Add(newClient);

                resp.Success = true;
                resp.Result = "Cliente adicionado com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar o cliente";
            }

            return resp;
        }
    }
}
