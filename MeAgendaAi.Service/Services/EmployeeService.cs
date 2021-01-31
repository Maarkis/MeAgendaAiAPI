using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Company;
using MeAgendaAi.Domain.EpModels.Employee;
using System;
using System.Collections.Generic;
using System.Text;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.EpModels.EmployeeWorkHours;
using MeAgendaAi.Domain.Validators.Employee;
using System.Linq;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Enums;

namespace MeAgendaAi.Service.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;
        private IServiceEmployeeRepository _serviceEmployeeRepository;
        private IEmployeeWorkHoursService _employeeWorkHoursService;
        private IServiceRepository _serviceRepository;
        private IUserService _userService;
        private ICompanyRepository _companyRepository;
        private ICompanyService _companyService;
        private ILocationService _locationService;
        private IPhoneNumberService _phoneNumberService;

        public EmployeeService(IEmployeeRepository employeeRepository, 
            IServiceEmployeeRepository serviceEmployeeRepository,
            IEmployeeWorkHoursService employeeWorkHoursService,
            IServiceRepository serviceRepository, IUserService userService,
            ICompanyRepository companyRepository, ICompanyService companyService,
            ILocationService locationService, IPhoneNumberService phoneNumberService) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceEmployeeRepository = serviceEmployeeRepository;
            _employeeWorkHoursService = employeeWorkHoursService;
            _serviceRepository = serviceRepository;
            _userService = userService;
            _companyRepository = companyRepository;
            _companyService = companyService;
            _locationService = locationService;
            _phoneNumberService = phoneNumberService;
        }

        public ResponseModel AddEmployee(AddEmployeeModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var validateEmployee = new AddEmployeeModelValidator().Validate(model);
                if (validateEmployee.IsValid) {

                    // verificar se a empresa existe no sistema.
                    var company = _companyRepository.GetById(Guid.Parse(model.CompanyId));
                    if(company != null)
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

                        List<Roles> roles = new List<Roles>();
                        roles.Add(Roles.Funcionario);
                        var userResponse = _userService.CreateUserFromModel(userModel, roles);
                        if (userResponse.Success)
                        {
                            User newUser = userResponse.Result as User;
                            Employee employee = new Employee
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
                                User = newUser,
                            };
                            _employeeRepository.Add(employee);

                            ResponseModel send = _userService.SendEmailConfirmation(userModel.Email).Result;

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
            ResponseModel resp = new ResponseModel();
            try
            {
                var validateEmployee = new EditEmployeeModelValidator().Validate(model);
                if (validateEmployee.IsValid)
                {
                    var employee = _employeeRepository.GetEmployeeByUserId(Guid.Parse(model.UsuarioId));
                    if (employee != null)
                    {
                        EditUserModel editUserModel = new EditUserModel
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
                            User employeeUser = userResponse.Result as User;

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
            catch(Exception e)
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
                        model.ServicesIds.ForEach(serviceId => {
                            ServiceEmployee serviceEmployee = new ServiceEmployee
                            {
                                EmployeeServiceId = Guid.NewGuid(),
                                ServiceId = Guid.Parse(serviceId),
                                EmployeeId = Guid.Parse(model.EmployeeId),
                                CreatedAt = DateTimeUtil.UtcToBrasilia(),
                                LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                            };
                            _serviceEmployeeRepository.Add(serviceEmployee);
                        });
                     
                        resp.Success = true;
                        resp.Message = "Serviços adicionados ao funcionário com sucesso";
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
            catch (Exception)
            {
                resp.Message = "Não foi possível adicionar o serviço ao funcionário";
            }

            return resp;
        }

        public ResponseModel GetEmployeeServices(string employeeId)
        {
            var resp = new ResponseModel();

            try
            {
                List<Domain.Entities.Services> services = _employeeRepository.GetEmployeeServicesByEmployeeId(Guid.Parse(employeeId));
                List<GetCompanyServicesModel> servicesEmployee = new List<GetCompanyServicesModel>();
                services.ForEach(service => {
                    GetCompanyServicesModel serviceEmployee = new GetCompanyServicesModel
                    {
                        ServiceId = service.ServiceId.ToString(),
                        Name = service.Name
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
            ResponseModel response = new ResponseModel();

            if (GuidUtil.IsGuidValid(userId))
            {
                var employee = _employeeRepository.GetEmployeeByUserId(Guid.Parse(userId));
                if(employee != null)
                {
                    response = GetEmployeeInfoComplete(employee.EmployeeId);
                }
                else
                {
                    response.Message = "Funciónário não encontrado";
                }
            }
            else
            {
                response.Message = "Guid inválido";
            }

            return response;
        }

        public ResponseModel GetEmployeeInfo(string employeeId)
        {
            ResponseModel response = new ResponseModel();

            if (GuidUtil.IsGuidValid(employeeId))
            {
                response = GetEmployeeInfoComplete(Guid.Parse(employeeId));
            }
            else
            {
                response.Message = "Guid inválido";
            }

            return response;
        }

        private ResponseModel GetEmployeeInfoComplete(Guid employeeId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var employeeComplete = _employeeRepository.GetByIdComplete(employeeId);
                if(employeeComplete != null)
                {
                    GetEmployeeInfoCompleteModel model = new GetEmployeeInfoCompleteModel
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


            }catch(Exception e)
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
                Link = _companyService.GetCompanyLink(company.CompanyId)
            };
        }
        private List<GetCompanyByIdCompleteServiceModel> GetServicesModelFromServices(List<ServiceEmployee> employeeServices)
        {
            List<GetCompanyByIdCompleteServiceModel> serviceModels = new List<GetCompanyByIdCompleteServiceModel>();

            if(employeeServices != null)
            {
                employeeServices.ForEach(employeeService => {
                    var service = employeeService.Service;
                    GetCompanyByIdCompleteServiceModel serviceModel = new GetCompanyByIdCompleteServiceModel
                    {
                        ServiceId = service.ServiceId.ToString(),
                        ServiceName = service.Name,
                        ServiceDuration = service.DurationMinutes
                    };
                    serviceModels.Add(serviceModel);
                });
            }

            return serviceModels;
        }

        public string GetEmployeeLink(Guid employeeId)
        {
            return $"funcionario/id={employeeId}";
        }
        public ResponseModel AddWorkHoursToEmployee(AddEmployeeWorkHoursModel model, string userEmail)
        {
            var response = new ResponseModel();

            var employee = _employeeRepository.GetEmployeeByUserEmail(userEmail);
            if(employee != null)
            {
                response = _employeeWorkHoursService.AddEmployeeWorkhours(model, employee);
            }
            else
            {
                response.Message = "Funcionário não encontrado";
            }

            return response;
        }

        public ResponseModel GetEmployeeAvailableHours(string employeeId, string serviceId, string date)
        {
            var employee = _employeeRepository.GetById(Guid.Parse(employeeId));
            var service = _serviceRepository.GetById(Guid.Parse(serviceId));

            DateTime dateTime = new DateTime();
            bool ok = DateTime.TryParse(date, out dateTime); //colocar validação do formato de data no validator
            return _employeeWorkHoursService.GetAvailableEmployeeWorkHours(dateTime, employee, service);
        }

        public ResponseModel GetEmployeeMonthSchedule(string userId, int ano, int mes)
        {
            return _employeeWorkHoursService.GetEmployeeMonthSchedule(userId, ano, mes);
        }

        public ResponseModel GetEmployeesByServiceId(string serviceId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                if (GuidUtil.IsGuidValid(serviceId))
                {
                    List<EmployeeFavInfoModel> employeeModels = new List<EmployeeFavInfoModel>();

                    var employees = _employeeRepository.GetEmployeesByServiceId(Guid.Parse(serviceId));
                    employees.ForEach(employee => {
                        var employeeModel = EmployeeToEmployeeFavModel(employee);
                        if(employeeModel != null)
                        {
                            employeeModels.Add(employeeModel);
                        }
                    });
                    response.Message = "Funcinários encontrados!";
                    response.Result = employeeModels;
                    response.Success = true;
                }
                else
                {
                    response.Message = "Guid inválido";
                }
            }catch(Exception e)
            {
                response.Message = $"Não foi possível buscar os funcionários. {e.InnerException.Message}";
            }

            return response;
        }

        public ResponseModel GetEmployeesByCompanyId(string companyId)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                if (GuidUtil.IsGuidValid(companyId))
                {
                    List<EmployeeFavInfoModel> employeeModels = new List<EmployeeFavInfoModel>();

                    var employees = _employeeRepository.GetEmployeesByCompanyId(Guid.Parse(companyId));
                    employees.ForEach(employee => {
                        var employeeModel = EmployeeToEmployeeFavModel(employee);
                        if (employeeModel != null)
                        {
                            employeeModels.Add(employeeModel);
                        }
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
        private EmployeeFavInfoModel EmployeeToEmployeeFavModel(Employee employee)
        {
            List<EmployeeFavServiceModel> serviceModels = new List<EmployeeFavServiceModel>();
            if (employee.EmployeeServices != null && employee.EmployeeServices.Count > 0)
            {
                employee.EmployeeServices.ForEach(serviceEp => {
                    var serviceModel = new EmployeeFavServiceModel
                    {
                        ServiceId = serviceEp.ServiceId.ToString(),
                        ServiceName = serviceEp.Service.Name
                    };
                    serviceModels.Add(serviceModel);
                });
            }

            var companyModel = new CompanyFavInfoModel
            {
                CompanyId = employee.CompanyId.ToString(),
                Name = employee.Company?.User?.Name ?? String.Empty,
                Image = employee.Company?.User?.Image ?? String.Empty,
                Email = employee.Company?.User?.Email ?? String.Empty,
                Descricao = employee?.Company?.Descricao ?? String.Empty,
                Link = _companyService.GetCompanyLink(employee.CompanyId)
            };

            EmployeeFavInfoModel employeeModel = new EmployeeFavInfoModel
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
