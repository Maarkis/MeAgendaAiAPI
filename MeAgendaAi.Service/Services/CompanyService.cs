using System;
using System.Collections.Generic;
using System.Linq;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Company;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.Validators.Company;

namespace MeAgendaAi.Service.Services
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILocationService _locationService;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IPolicyRepository _policyRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserService _userService;
        private IUserRepository _userRepository;

        public CompanyService(ICompanyRepository companyRepository, IUserRepository userRepository,
            IEmployeeRepository employeeRepository, IServiceRepository serviceRepository,
            IPolicyRepository policyRepository, IUserService userService,
            ILocationService locationService, IPhoneNumberService phoneNumberService) : base(companyRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _serviceRepository = serviceRepository;
            _policyRepository = policyRepository;
            _userService = userService;
            _locationService = locationService;
            _phoneNumberService = phoneNumberService;
        }

        public ResponseModel AddCompany(AddCompanyModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var validateCompany = new AddCompanyModelValidator().Validate(model);
                if (validateCompany.IsValid)
                {
                    var userModel = new AddUserModel
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password,
                        Locations = model.Locations,
                        PhoneNumbers = model.PhoneNumbers,
                        Imagem = model.Imagem
                    };

                    var roles = new List<Roles>();
                    roles.Add(Roles.UsuarioEmpresa);
                    var userResponse = _userService.CreateUserFromModel(userModel, roles);
                    if (userResponse.Success)
                    {
                        var newUser = userResponse.Result as User;
                        var companyId = Guid.NewGuid();
                        var policy = new Policy
                        {
                            PolicyId = Guid.NewGuid(),
                            CompanyId = companyId,
                            LimitCancelHours = model.LimitCancelHours
                        };

                        var newCompany = new Company
                        {
                            CompanyId = companyId,
                            CNPJ = model.CNPJ,
                            Policy = policy,
                            Descricao = model.Descricao,
                            CreatedAt = DateTimeUtil.UtcToBrasilia(),
                            LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                            UserId = newUser.UserId,
                            User = newUser
                        };
                        _companyRepository.Add(newCompany);

                        var send = _userService.SendEmailConfirmation(userModel.Email).Result;

                        resp.Success = true;
                        resp.Result = $"{newUser.UserId}";
                        resp.Message = "Empresa Adicionada com sucesso!";
                    }
                    else
                    {
                        resp = userResponse;
                    }
                }
                else
                {
                    resp.Message = validateCompany.Errors.FirstOrDefault().ErrorMessage;
                }
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível adicionar a empresa";
            }

            return resp;
        }

        public ResponseModel EditCompany(EditCompanyModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var validateCompany = new EditCompanyModelValidator().Validate(model);
                if (validateCompany.IsValid)
                {
                    var company = _companyRepository.GetCompanyByUserId(Guid.Parse(model.UserId));
                    if (company != null)
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
                            var companyUser = userResponse.Result as User;

                            company.Policy.LimitCancelHours = model.LimitCancelHours;
                            company.Policy.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                            company.Policy.UpdatedBy = companyUser.UserId;

                            company.Descricao = model.Descricao;
                            company.User = companyUser;

                            company.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                            company.UpdatedBy = companyUser.UserId;

                            _companyRepository.Edit(company);

                            resp.Success = true;
                            resp.Message = "Empresa editada com sucesso";
                        }
                        else
                        {
                            resp = userResponse;
                        }
                    }
                    else
                    {
                        resp.Message = "Empresa não econtrada no banco de dados";
                    }
                }
                else
                {
                    resp.Message = validateCompany.Errors.FirstOrDefault().ErrorMessage;
                }
            }
            catch (Exception e)
            {
                resp.Message = "Não foi possível editar a empresa";
            }

            return resp;
        }

        public ResponseModel CreateServiceForCompany(AddMultipleServicesModel model)
        {
            var resp = new ResponseModel();
            try
            {
                var company = _companyRepository.GetById(Guid.Parse(model.CompanyId));
                if (company == null)
                {
                    resp.Result = "Não foi possível encontrar a empresa";
                    return resp;
                }

                if (model.Services != null && model.Services.Count > 0)
                {
                    if (model.Services.All(x => x.DurationMinutes > 0))
                    {
                        if (model.Services.All(x => x.Name != null && x.Name != string.Empty))
                        {
                            var servicesCompany = _serviceRepository.GetServicesByCompanyId(company.CompanyId);
                            model.Services.ForEach(serviceModel =>
                            {
                                // não adicionar dois serviços com o mesmo nome na empresa
                                if (servicesCompany.All(x =>
                                    x.Name.ToLowerInvariant() != serviceModel.Name.ToLowerInvariant()))
                                {
                                    var service = new Domain.Entities.Services
                                    {
                                        ServiceId = Guid.NewGuid(),
                                        CompanyId = company.CompanyId,
                                        Name = serviceModel.Name,
                                        DurationMinutes = serviceModel.DurationMinutes,
                                        CreatedAt = DateTimeUtil.UtcToBrasilia(),
                                        LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                                    };
                                    _serviceRepository.Add(service);
                                }
                            });

                            resp.Success = true;
                            resp.Message = "Serviços adicionados à empresa com sucesso";
                        }
                        else
                        {
                            resp.Message = "Adicione um nome para o serviço";
                        }
                    }
                    else
                    {
                        resp.Message = "Tempo de duração do serviço deve ser maior do que zero";
                    }
                }
                else
                {
                    resp.Message = "Lista de serviços vazia, adicione algum serviço";
                }
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível adicionar o serviço";
            }

            return resp;
        }

        public ResponseModel GetCompanyServices(string companyId)
        {
            var resp = new ResponseModel();

            try
            {
                var services = _serviceRepository.GetServicesByCompanyId(Guid.Parse(companyId));
                var companyServices = new List<GetCompanyServicesModel>();
                services.ForEach(service =>
                {
                    var companyService = new GetCompanyServicesModel
                    {
                        ServiceId = service.ServiceId.ToString(),
                        Name = service.Name,
                        DurationMinutes = service.DurationMinutes
                    };
                    companyServices.Add(companyService);
                });
                resp.Success = true;
                resp.Result = companyServices;
                resp.Message = "Serviços da empresa selecionados!";
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível selecionar os serviços da empresa";
            }

            return resp;
        }

        public ResponseModel UpdatePolicy(UpdatePolicyModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var company = _companyRepository.GetCompanyWithPolicyById(Guid.Parse(model.CompanyId));
                if (company == null)
                {
                    resp.Result = "Não foi posível encontrar a empresa ou a política requisitadas";
                    return resp;
                }

                var policy = company.Policy;
                policy.LimitCancelHours = model.LimitCancelHours;
                policy.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();

                _policyRepository.Edit(policy);

                resp.Success = true;
                resp.Message = "Atualizado com sucesso";
                return resp;
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível alterar a política da empresa";
            }

            return resp;
        }

        public ResponseModel GetCompanyInfoPerfil(string userId)
        {
            var responseModel = new ResponseModel();

            if (GuidUtil.IsGuidValid(userId))
            {
                var company = _companyRepository.GetCompanyByUserId(Guid.Parse(userId));
                if (company != null)
                    responseModel = GetCompanyComplete(company.CompanyId);
                else
                    responseModel.Message = "Não foi possível encontrar a empresa";
            }
            else
            {
                responseModel.Message = "Guid inválido";
            }

            return responseModel;
        }

        public ResponseModel GetCompanyInfo(string companyId)
        {
            var responseModel = new ResponseModel();

            if (GuidUtil.IsGuidValid(companyId))
                responseModel = GetCompanyComplete(Guid.Parse(companyId));
            else
                responseModel.Message = "Guid inválido";

            return responseModel;
        }

        public string GetCompanyLink(Guid companyId)
        {
            return _companyRepository.GetCompanyLink(companyId);
            //return $"http://localhost:4200/perfil_empresa/{companyId}";
        }

        private ResponseModel GetCompanyComplete(Guid companyId)
        {
            var resp = new ResponseModel();

            try
            {
                var companyComplete = _companyRepository.GetCompanyByIdComplete(companyId);
                if (companyComplete != null)
                {
                    var companyServices = new List<GetCompanyByIdCompleteServiceModel>();
                    companyComplete.Services.ForEach(service =>
                    {
                        var companyService = new GetCompanyByIdCompleteServiceModel
                        {
                            ServiceId = service.ServiceId.ToString(),
                            Name = service.Name,
                            DurationMinutes = service.DurationMinutes
                        };
                        companyServices.Add(companyService);
                    });

                    var companyEmployees = new List<GetCompanyByIdCompleteEmployeeModel>();
                    companyComplete.Employees.ForEach(employee =>
                    {
                        var employeeServices = new List<GetCompanyByIdCompleteServiceModel>();
                        employee.EmployeeServices.ForEach(eService =>
                        {
                            var employeeService = new GetCompanyByIdCompleteServiceModel
                            {
                                ServiceId = eService.Service.ServiceId.ToString(),
                                DurationMinutes = eService.Service.DurationMinutes,
                                Name = eService.Service.Name
                            };
                            employeeServices.Add(employeeService);
                        });

                        var companyEmployee = new GetCompanyByIdCompleteEmployeeModel
                        {
                            EmployeeId = employee.EmployeeId.ToString(),
                            EmplyeeName = employee.User.Name,
                            Image = employee.User.Image,
                            Link = _employeeRepository.GetEmployeeLink(employee.EmployeeId),
                            Descricao = employee.Descricao,
                            IsManager = employee.IsManager,
                            EmployeeServices = employeeServices
                        };
                        companyEmployees.Add(companyEmployee);
                    });

                    var model = new GetCompanyByIdCompleteModel
                    {
                        CompanyId = companyComplete.CompanyId,
                        CompanyName = companyComplete.User.Name,
                        Descricao = companyComplete.Descricao,
                        Email = companyComplete.User.Email,
                        Image = companyComplete.User.Image,
                        Link = GetCompanyLink(companyComplete.CompanyId),
                        LimitCancelHours = companyComplete.Policy.LimitCancelHours,
                        CNPJ = companyComplete.CNPJ,
                        CompanyServices = companyServices,
                        Employees = companyEmployees,
                        Locations = _locationService.UserLocationsToBasicLocationModel(companyComplete.UserId),
                        PhoneNumbers = _phoneNumberService.UserPhoneNumbersToPhoneNumberModel(companyComplete.UserId)
                    };

                    resp.Success = true;
                    resp.Message = "Informações da empresa";
                    resp.Result = model;
                }
                else
                {
                    resp.Message = "Empresa não encontrada";
                }
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível coletar as informações da empresa";
            }

            return resp;
        }
    }
}