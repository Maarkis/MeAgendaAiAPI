using System;
using System.Collections.Generic;
using System.Linq;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Company;
using MeAgendaAi.Domain.EpModels.Employee;
using MeAgendaAi.Domain.EpModels.EmployeeWorkHours;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.Validators.Employee;

namespace MeAgendaAi.Service.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmployeeWorkHoursService _employeeWorkHoursService;
        private readonly ILocationService _locationService;
        private readonly IPhoneNumberService _phoneNumberService;
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserService _userService;
        private IServiceEmployeeRepository _serviceEmployeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository,
            IServiceEmployeeRepository serviceEmployeeRepository,
            IEmployeeWorkHoursService employeeWorkHoursService,
            IServiceRepository serviceRepository, IUserService userService,
            ICompanyRepository companyRepository,
            ILocationService locationService, IPhoneNumberService phoneNumberService) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceEmployeeRepository = serviceEmployeeRepository;
            _employeeWorkHoursService = employeeWorkHoursService;
            _serviceRepository = serviceRepository;
            _userService = userService;
            _companyRepository = companyRepository;
            _locationService = locationService;
            _phoneNumberService = phoneNumberService;
        }

        public ResponseModel AddEmployee(AddEmployeeModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var validateEmployee = new AddEmployeeModelValidator().Validate(model);
                if (validateEmployee.IsValid)
                {
                    // verificar se a empresa existe no sistema.
                    var company = _companyRepository.GetById(Guid.Parse(model.CompanyId));
                    if (company != null)
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
                        roles.Add(Roles.Funcionario);
                        var userResponse = _userService.CreateUserFromModel(userModel, roles);
                        if (userResponse.Success)
                        {
                            var newUser = userResponse.Result as User;
                            var employee = new Employee
                            {
                                EmployeeId = Guid.NewGuid(),
                                CompanyId = company.CompanyId,
                                IsManager = model.IsManager,
                                Descricao = model.Descricao,
                                CPF = model.CPF,
                                RG = model.RG,
                                CreatedAt = DateTimeUtil.UtcToBrasilia(),
                                LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                                UserId = newUser.UserId,
                                User = newUser
                            };
                            _employeeRepository.Add(employee);

                            var send = _userService.SendEmailConfirmation(userModel.Email).Result;

                            resp.Success = true;
                            resp.Result = $"{employee.EmployeeId}";
                            resp.Message = "Funcionário adicionado com sucesso!";
                        }
                        else
                        {
                            resp = userResponse;
                        }
                    }
                    else
                    {
                        resp.Message = "Empresa não encontrada no banco de dados";
                    }
                }
                else
                {
                    resp.Message = validateEmployee.Errors.FirstOrDefault().ErrorMessage;
                }
            }
            catch (Exception e)
            {
                resp.Message = "Não foi possível adicionar o funcionário";
            }

            return resp;
        }

        public ResponseModel EditEmployee(EditEmployeeModel model)
        {
            var resp = new ResponseModel();
            try
            {
                var validateEmployee = new EditEmployeeModelValidator().Validate(model);
                if (validateEmployee.IsValid)
                {
                    var employee = _employeeRepository.GetEmployeeByUserId(Guid.Parse(model.UsuarioId));
                    if (employee != null)
                    {
                        var editUserModel = new EditUserModel
                        {
                            UsuarioId = model.UsuarioId,
                            Name = model.Name,
                            Locations = model.Locations,
                            PhoneNumbers = model.PhoneNumbers,
                            Imagem = model.Imagem
                        };

                        var userResponse = _userService.EditUserFromModel(editUserModel);
                        if (userResponse.Success)
                        {
                            var employeeUser = userResponse.Result as User;

                            employee.RG = model.RG;
                            employee.IsManager = model.IsManager;
                            employee.Descricao = model.Descricao;
                            employee.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                            employee.UpdatedBy = employeeUser.UserId;
                            employee.User = employeeUser;
                            _employeeRepository.Edit(employee);

                            resp.Success = true;
                            resp.Message = "Funcionário editado com sucesso";
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
                    resp.Message = validateEmployee.Errors.FirstOrDefault().ErrorMessage;
                }
            }
            catch (Exception e)
            {
                resp.Message = "Não foi possível editar o funcionário";
            }

            return resp;
        }

        public ResponseModel AddServiceToEmployee(AddServiceToEmployeeModel model)
        {
            var resp = new ResponseModel();

            try
            {
                if (model.ServicesIds != null && model.ServicesIds.Count > 0)
                {
                    if (GuidUtil.IsGuidValid(model.EmployeeId) && model.ServicesIds.All(x => GuidUtil.IsGuidValid(x)))
                    {
                        var employee = _employeeRepository.GetEmployeeByIdWithServices(Guid.Parse(model.EmployeeId));
                        if (employee != null)
                        {
                            //var ess = employee.EmployeeServices;
                            //ess.ForEach(es => {
                            //    _serviceEmployeeRepository.Remove(es);
                            //});

                            var ses = new List<ServiceEmployee>();

                            model.ServicesIds.ForEach(serviceId =>
                            {
                                var service = _serviceRepository.GetById(Guid.Parse(serviceId));

                                var serviceEmployee = new ServiceEmployee
                                {
                                    EmployeeServiceId = Guid.NewGuid(),
                                    ServiceId = Guid.Parse(serviceId),
                                    EmployeeId = Guid.Parse(model.EmployeeId),
                                    Employee = employee,
                                    Service = service,
                                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                                    UpdatedBy = employee.UserId
                                };
                                //_serviceEmployeeRepository.Add(serviceEmployee);
                                ses.Add(serviceEmployee);
                            });

                            employee.EmployeeServices = ses;
                            _employeeRepository.Edit(employee);

                            resp.Success = true;
                            resp.Message = "Serviços adicionados ao funcionário com sucesso";
                        }
                        else
                        {
                            resp.Message = "Funcionário não encontrado";
                        }
                    }
                    else
                    {
                        resp.Message = "Guid inválido";
                    }
                }
                else
                {
                    resp.Message = "Lista de serviços vazia, adicione algum serviço";
                }
            }
            catch (Exception e)
            {
                resp.Message =
                    $"Não foi possível adicionar o serviço ao funcionário: {e.Message} / {e.InnerException?.Message}";
            }

            return resp;
        }

        public ResponseModel GetEmployeeServices(string employeeId)
        {
            var resp = new ResponseModel();

            try
            {
                var services = _employeeRepository.GetEmployeeServicesByEmployeeId(Guid.Parse(employeeId));
                var servicesEmployee = new List<GetCompanyServicesModel>();
                services.ForEach(service =>
                {
                    var serviceEmployee = new GetCompanyServicesModel
                    {
                        ServiceId = service.ServiceId.ToString(),
                        Name = service.Name,
                        DurationMinutes = service.DurationMinutes
                    };
                    servicesEmployee.Add(serviceEmployee);
                });
                resp.Success = true;
                resp.Result = servicesEmployee;
                resp.Message = "Serviços do funcionário selecionados!";
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível adicionar o serviço ao funcionário";
            }

            return resp;
        }

        public ResponseModel GetEmployeePerfilInfo(string userId)
        {
            var response = new ResponseModel();

            if (GuidUtil.IsGuidValid(userId))
            {
                var employee = _employeeRepository.GetEmployeeByUserId(Guid.Parse(userId));
                if (employee != null)
                    response = GetEmployeeInfoComplete(employee.EmployeeId);
                else
                    response.Message = "Funciónário não encontrado";
            }
            else
            {
                response.Message = "Guid inválido";
            }

            return response;
        }

        public ResponseModel GetEmployeeInfo(string employeeId)
        {
            var response = new ResponseModel();

            if (GuidUtil.IsGuidValid(employeeId))
                response = GetEmployeeInfoComplete(Guid.Parse(employeeId));
            else
                response.Message = "Guid inválido";

            return response;
        }

        public string GetEmployeeLink(Guid employeeId)
        {
            //return $"http://localhost:4200/perfil_funcionario/{employeeId}";
            return _employeeRepository.GetEmployeeLink(employeeId);
        }

        public ResponseModel AddWorkHoursToEmployee(AddEmployeeWorkHoursModel model, string userEmail)
        {
            var response = new ResponseModel();

            var employee = _employeeRepository.GetEmployeeByUserEmail(userEmail);
            if (employee != null)
                response = _employeeWorkHoursService.AddEmployeeWorkhours(model, employee);
            else
                response.Message = "Funcionário não encontrado";

            return response;
        }

        public ResponseModel GetEmployeeAvailableHours(string employeeId, string serviceId, string date)
        {
            var employee = _employeeRepository.GetById(Guid.Parse(employeeId));
            var service = _serviceRepository.GetById(Guid.Parse(serviceId));

            var dateTime = new DateTime();
            var ok = DateTime.TryParse(date, out dateTime); //colocar validação do formato de data no validator
            //return _employeeWorkHoursService.GetAvailableEmployeeWorkHours(dateTime, employee, service);
            return _employeeWorkHoursService.GetWorkHoursMock(dateTime, employee, service);
        }

        public ResponseModel GetEmployeeMonthSchedule(string userId, int ano, int mes)
        {
            return _employeeWorkHoursService.GetEmployeeMonthSchedule(userId, ano, mes);
        }

        public ResponseModel GetEmployeesByServiceId(string serviceId)
        {
            var response = new ResponseModel();

            try
            {
                if (GuidUtil.IsGuidValid(serviceId))
                {
                    var employeeModels = new List<EmployeeFavInfoModel>();

                    var employees = _employeeRepository.GetEmployeesByServiceId(Guid.Parse(serviceId));
                    employees.ForEach(employee =>
                    {
                        var employeeModel = EmployeeToEmployeeFavModel(employee);
                        if (employeeModel != null) employeeModels.Add(employeeModel);
                    });
                    response.Message = "Funcinários encontrados!";
                    response.Result = employeeModels;
                    response.Success = true;
                }
                else
                {
                    response.Message = "Guid inválido";
                }
            }
            catch (Exception e)
            {
                response.Message = $"Não foi possível buscar os funcionários. {e.InnerException.Message}";
            }

            return response;
        }

        public ResponseModel GetEmployeesByCompanyId(string companyId)
        {
            var response = new ResponseModel();

            try
            {
                if (GuidUtil.IsGuidValid(companyId))
                {
                    var employeeModels = new List<EmployeeFavInfoModel>();

                    var employees = _employeeRepository.GetEmployeesByCompanyId(Guid.Parse(companyId));
                    employees.ForEach(employee =>
                    {
                        var employeeModel = EmployeeToEmployeeFavModel(employee);
                        if (employeeModel != null) employeeModels.Add(employeeModel);
                    });
                    response.Message = "Funcinários encontrados!";
                    response.Result = employeeModels;
                    response.Success = true;
                }
                else
                {
                    response.Message = "Guid inválido";
                }
            }
            catch (Exception e)
            {
                response.Message = $"Não foi possível buscar os funcionários. {e.InnerException.Message}";
            }

            return response;
        }

        private ResponseModel GetEmployeeInfoComplete(Guid employeeId)
        {
            var response = new ResponseModel();

            try
            {
                var employeeComplete = _employeeRepository.GetByIdComplete(employeeId);
                if (employeeComplete != null)
                {
                    var model = new GetEmployeeInfoCompleteModel
                    {
                        EmployeeId = employeeId.ToString(),
                        Name = employeeComplete.User.Name,
                        Image = employeeComplete.User.Image,
                        Link = GetEmployeeLink(employeeComplete.EmployeeId),
                        CPF = employeeComplete.CPF,
                        RG = employeeComplete.RG,
                        Descricao = employeeComplete.Descricao,
                        DataCadastro = employeeComplete.User.CreatedAt.ToString(),
                        IsManager = employeeComplete.IsManager,
                        Locations = _locationService.UserLocationsToBasicLocationModel(employeeComplete.UserId),
                        PhoneNumbers = _phoneNumberService.UserPhoneNumbersToPhoneNumberModel(employeeComplete.UserId),
                        Company = CompanyToCompanyBasicInfoModel(employeeComplete.Company),
                        EmployeeServices = GetServicesModelFromServices(employeeComplete.EmployeeServices)
                    };

                    response.Success = true;
                    response.Message = "Informações do funcionário";
                    response.Result = model;
                }
                else
                {
                    response.Message = "Funciomário não encontrado";
                }
            }
            catch (Exception e)
            {
                response.Message = $"Não foi possível recuperar as informações do funcionário. \n {e.Message}";
            }

            return response;
        }

        private CompanyBasicInfoModel CompanyToCompanyBasicInfoModel(Company company)
        {
            return new CompanyBasicInfoModel
            {
                CompanyId = company.CompanyId.ToString(),
                Image = company.User.Image,
                Name = company.User.Name,
                Link = _companyRepository.GetCompanyLink(company.CompanyId)
            };
        }

        private List<GetCompanyByIdCompleteServiceModel> GetServicesModelFromServices(
            List<ServiceEmployee> employeeServices)
        {
            var serviceModels = new List<GetCompanyByIdCompleteServiceModel>();

            if (employeeServices != null)
                employeeServices.ForEach(employeeService =>
                {
                    var service = employeeService.Service;
                    var serviceModel = new GetCompanyByIdCompleteServiceModel
                    {
                        ServiceId = service.ServiceId.ToString(),
                        Name = service.Name,
                        DurationMinutes = service.DurationMinutes
                    };
                    serviceModels.Add(serviceModel);
                });

            return serviceModels;
        }

        private EmployeeFavInfoModel EmployeeToEmployeeFavModel(Employee employee)
        {
            var serviceModels = new List<EmployeeFavServiceModel>();
            if (employee.EmployeeServices != null && employee.EmployeeServices.Count > 0)
                employee.EmployeeServices.ForEach(serviceEp =>
                {
                    var serviceModel = new EmployeeFavServiceModel
                    {
                        ServiceId = serviceEp.ServiceId.ToString(),
                        ServiceName = serviceEp.Service.Name
                    };
                    serviceModels.Add(serviceModel);
                });

            var companyModel = new CompanyFavInfoModel
            {
                CompanyId = employee.CompanyId.ToString(),
                Name = employee.Company?.User?.Name ?? string.Empty,
                Image = employee.Company?.User?.Image ?? string.Empty,
                Email = employee.Company?.User?.Email ?? string.Empty,
                Descricao = employee?.Company?.Descricao ?? string.Empty,
                Link = _companyRepository.GetCompanyLink(employee.CompanyId)
            };

            var employeeModel = new EmployeeFavInfoModel
            {
                EmployeeId = employee.EmployeeId.ToString(),
                Link = GetEmployeeLink(employee.EmployeeId),
                Name = employee.User.Name,
                Email = employee.User.Email,
                Descricao = employee.Descricao,
                Image = employee.User.Image,
                Services = serviceModels,
                Company = companyModel
            };

            return employeeModel;
        }
    }
}