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
using FluentValidation.Results;
using MeAgendaAi.Domain.Interfaces.Services.Email;
using FluentValidation;
using System.Threading.Tasks;

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

        public async Task<ResponseModel> AddClient(AddClientModel model)
        {
            ResponseModel resp = new ResponseModel();
            try
            {
                ValidationResult validateClient = new AddClientModelValidator().Validate(model);
                if (validateClient.IsValid)
                {
                    AddUserModel userModel = new AddUserModel
                    {
                        Email = model.Email,
                        Name = model.Name,
                        Imagem = model.Imagem,
                        Locations = model.Locations,
                        PhoneNumbers = model.PhoneNumbers,
                        Password = model.Password,
                        Verified = false
                    };

                    List<Roles> roles = new List<Roles>();
                    roles.Add(Roles.Cliente);
                    ResponseModel userResponse = _userService.CreateUserFromModel(userModel, roles);
                    if (userResponse.Success)
                    {
                        User newUser = userResponse.Result as User;
                        Client newClient = new Client
                        {
                            ClientId = Guid.NewGuid(),
                            UserId = newUser.UserId,
                            CreatedAt = DateTimeUtil.UtcToBrasilia(),
                            CPF = model.CPF,
                            RG = model.RG,
                            LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                            User = newUser

                        };
                        _clientRepository.Add(newClient);

                        ResponseModel send = await _userService.SendEmailConfirmation(userModel.Email);                        

                        resp.Success = true;
                        resp.Result = $"{newUser.UserId}";
                        resp.Message = "Cliente adicionado com sucesso";
                    }
                    else
                    {
                        resp = userResponse;
                    }
                }
                else
                {
                    resp.Message = validateClient.Errors.FirstOrDefault().ErrorMessage;
                }
            }
            catch (Exception e)
            {
                resp.Message = "Não foi possível adicionar o cliente";
            }

            return resp;
        }

        public ResponseModel EditClient(EditClientModel model)
        {
            var resp = new ResponseModel();

            try
            {
                ValidationResult validateClient = new EditClientModelValidator().Validate(model);
                if (validateClient.IsValid)
                {
                    Client client = _clientRepository.GetClientByUserId(Guid.Parse(model.UserId));
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

                        ResponseModel userResponse = _userService.EditUserFromModel(editUserModel);
                        if (userResponse.Success)
                        {
                            User clientUser = userResponse.Result as User;

                            client.RG = model.RG;
                            client.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                            client.UpdatedBy = clientUser.UserId;
                            client.User = clientUser;
                            _clientRepository.Edit(client);

                            resp.Success = true;
                            resp.Message = "Cliente editado com sucesso";
                        }
                        else
                        {
                            resp = userResponse;
                        }
                    }
                    else
                    {
                        resp.Message = "Cliente não encontrado";
                    }
                }
                else
                {
                    resp.Message = validateClient.Errors.FirstOrDefault().ErrorMessage;
                }
            }
            catch (Exception e)
            {
                resp.Message = "Não foi possível editar o cliente";
            }

            return resp;
        }
    }
}
