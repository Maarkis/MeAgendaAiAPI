using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.User;
using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels.Client;
using MeAgendaAi.Domain.Validators.Client;
using System.Linq;

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

        public ResponseModel AddClient(AddClientModel model)
        {
            var resp = new ResponseModel();
            try
            {
                var validateClient = new AddClientModelValidator().Validate(model);
                if (validateClient.IsValid)
                {
                    var userModel = new AddUserModel
                    {
                        Email = model.Email,
                        Name = model.Name,
                        Imagem = model.Imagem,
                        Locations = model.Locations,
                        PhoneNumbers = model.PhoneNumbers,
                        Password = model.Password
                    };

                    List<Roles> roles = new List<Roles>();
                    roles.Add(Roles.Cliente);
                    var userResponse = _userService.CreateUserFromModel(userModel, roles);
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
                        resp.Result = $"{newUser.UserId}";
                    }
                    else
                    {
                        resp = userResponse;
                    }
                }
                else
                {
                    resp.Result = validateClient.Errors.FirstOrDefault().ToString();
                }
            }
            catch (Exception e)
            {
                resp.Result = "Não foi possível adicionar o cliente";
            }

            return resp;
        }

        public ResponseModel EditClient(EditClientModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var validateClient = new EditClientModelValidator().Validate(model);
                if (validateClient.IsValid)
                {
                    var client = _clientRepository.GetClientByUserId(Guid.Parse(model.UserId));
                    if (client != null)
                    {
                        EditUserModel editUserModel = new EditUserModel
                        {
                            UsuarioId = model.UserId,
                            Name = model.Name,
                            Locations = model.Locations,
                            PhoneNumbers = model.PhoneNumbers,
                            Imagem = model.Imagem
                        };

                        var userResponse = _userService.EditUserFromModel(editUserModel);
                        if (userResponse.Success)
                        {
                            User clientUser = userResponse.Result as User;

                            client.RG = model.RG;
                            client.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                            client.UpdatedBy = clientUser.UserId;
                            client.User = clientUser;
                            _clientRepository.Edit(client);

                            resp.Success = true;
                            resp.Result = "Cliente editado com sucesso";
                        }
                        else
                        {
                            resp = userResponse;
                        }
                    }
                    else
                    {
                        resp.Result = "Cliente não encontrado";
                    }
                }
                else
                {
                    resp.Result = validateClient.Errors.FirstOrDefault().ToString();
                }
            }
            catch (Exception e)
            {
                resp.Result = "Não foi possível editar o cliente";
            }

            return resp;
        }
    }
}
