using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Client;
using MeAgendaAi.Domain.EpModels.Company;
using MeAgendaAi.Domain.EpModels.Employee;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.Validators.Client;

namespace MeAgendaAi.Service.Services
{
    public class ClientService : BaseService<Client>, IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ICompanyService _companyService;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeService _employeeService;
        private readonly ILocationService _locationService;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IUserService _userService;

        public ClientService(IClientRepository clientRepository,
            IUserService userService, ILocationService locationService,
            IPhoneNumberService phoneNumberService, IEmployeeRepository employeeRepository,
            IEmployeeService employeeService, ICompanyService companyService) : base(clientRepository)
        {
            _clientRepository = clientRepository;
            _userService = userService;
            _locationService = locationService;
            _phoneNumberService = phoneNumberService;
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
            _companyService = companyService;
        }

        public async Task<ResponseModel> AddClient(AddClientModel model)
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
                        Password = model.Password,
                        Verified = false
                    };

                    var roles = new List<Roles>();
                    roles.Add(Roles.Cliente);
                    var userResponse = _userService.CreateUserFromModel(userModel, roles);
                    if (userResponse.Success)
                    {
                        var newUser = userResponse.Result as User;
                        var newClient = new Client
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

                        var send = await _userService.SendEmailConfirmation(userModel.Email);

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
                var validateClient = new EditClientModelValidator().Validate(model);
                if (validateClient.IsValid)
                {
                    var client = _clientRepository.GetClientByUserId(Guid.Parse(model.UserId));
                    if (client != null)
                    {
                        var editUserModel = new EditUserModel
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
                            var clientUser = userResponse.Result as User;

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

        public ResponseModel GetClientPerfilInfo(string userId)
        {
            var response = new ResponseModel();

            try
            {
                if (GuidUtil.IsGuidValid(userId))
                {
                    var cliente = _clientRepository.GetClientByUserId(Guid.Parse(userId));
                    if (cliente != null)
                    {
                        var clientModel = new GetClientPerfilInfoModel
                        {
                            Nome = cliente.User.Name,
                            Email = cliente.User.Email,
                            Imagem = cliente.User.Image,
                            CPF = cliente.CPF,
                            RG = cliente.RG,
                            DataCadastro = cliente.User.CreatedAt.ToString(),
                            Locations = _locationService.UserLocationsToBasicLocationModel(cliente.UserId),
                            PhoneNumbers = _phoneNumberService.UserPhoneNumbersToPhoneNumberModel(cliente.UserId)
                        };
                        response.Success = true;
                        response.Result = clientModel;
                        response.Message = "Informações do cliente";
                    }
                    else
                    {
                        response.Message = "Não foi possível encontrar o cliente";
                    }
                }
                else
                {
                    response.Message = "Guid inválido";
                }
            }
            catch (Exception e)
            {
                response.Message = $"Não foi possível receber as informações do cliente. \n {e.Message}";
            }

            return response;
        }

        public ResponseModel GetClientFavoriteEmployees(string userId)
        {
            var response = new ResponseModel();

            try
            {
                if (GuidUtil.IsGuidValid(userId))
                {
                    var client = _clientRepository.GetClientByUserId(Guid.Parse(userId));
                    if (client != null)
                    {
                        var employeesModel = new List<EmployeeFavInfoModel>();

                        var employees = _employeeRepository.GetEmployeesByClientId(client.ClientId);
                        if (employees != null && employees.Count > 0)
                            employees.ForEach(employee =>
                            {
                                var serviceModels = new List<EmployeeFavServiceModel>();
                                var employeeServices = employee.EmployeeServices?.Select(x => x.Service).ToList();
                                if (employeeServices != null && employeeServices.Count > 0)
                                    employeeServices.ForEach(service =>
                                    {
                                        var serviceModel = new EmployeeFavServiceModel
                                        {
                                            ServiceId = service.ServiceId.ToString(),
                                            ServiceName = service.Name
                                        };

                                        serviceModels.Add(serviceModel);
                                    });

                                var companyModel = new CompanyFavInfoModel
                                {
                                    CompanyId = employee.Company?.CompanyId.ToString(),
                                    Name = employee.Company?.User?.Name,
                                    Email = employee.Company?.User?.Email,
                                    Image = employee.Company?.User?.Image,
                                    Descricao = employee?.Company?.Descricao,
                                    Link = _companyService.GetCompanyLink(employee.CompanyId)
                                };

                                var employeeModel = new EmployeeFavInfoModel
                                {
                                    EmployeeId = employee.EmployeeId.ToString(),
                                    Link = _employeeService.GetEmployeeLink(employee.EmployeeId),
                                    Name = employee.User.Name,
                                    Email = employee.User.Email,
                                    Descricao = employee.Descricao,
                                    Image = employee.User.Image,
                                    Services = serviceModels,
                                    Company = companyModel
                                };
                            });

                        response.Success = true;
                        response.Message = "Funcionários do cliente";
                        response.Result = employees;
                    }
                    else
                    {
                        response.Message = "Usuário não encontrado";
                    }
                }
                else
                {
                    response.Message = "Guid inválido";
                }
            }
            catch (Exception e)
            {
                response.Message = $"Não foi possível recuperar os funcionários. \n{e.Message}";
            }

            return response;
        }
    }
}