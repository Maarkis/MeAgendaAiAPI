using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Company;
using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Validators.Company;
using System.Linq;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.EpModels.User;

namespace MeAgendaAi.Service.Services
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {
        private ICompanyRepository _companyRepository;
        private IUserRepository _userRepository;
        private IEmployeeRepository _employeeRepository;
        private IServiceRepository _serviceRepository;
        private IPolicyRepository _policyRepository;
        private IUserService _userService;
        public CompanyService(ICompanyRepository companyRepository, IUserRepository userRepository,
            IEmployeeRepository employeeRepository, IServiceRepository serviceRepository,
            IPolicyRepository policyRepository, IUserService userService) : base(companyRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _serviceRepository = serviceRepository;
            _policyRepository = policyRepository;
            _userService = userService;
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

                    List<Roles> roles = new List<Roles>();
                    roles.Add(Roles.UsuarioEmpresa);
                    var userResponse = _userService.CreateUserFromModel(userModel, roles);
                    if (userResponse.Success)
                    {
                        User newUser = userResponse.Result as User;
                        Guid companyId = Guid.NewGuid();
                        Policy policy = new Policy
                        {
                            PolicyId = Guid.NewGuid(),
                            CompanyId = companyId,
                            LimitCancelHours = model.LimitCancelHours
                        };

                        Company newCompany = new Company
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

                        ResponseModel send = _userService.SendEmailConfirmation(userModel.Email).Result;

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
                    resp.Result = validateCompany.Errors.FirstOrDefault().ToString();
                }
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar a empresa";
            }

            return resp;
        }


        public ResponseModel EditCompany(EditCompanyModel model)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                var validateCompany = new EditCompanyModelValidator().Validate(model);
                if (validateCompany.IsValid)
                {
                    Company company = _companyRepository.GetCompanyByUserId(Guid.Parse(model.UserId));
                    if(company != null)
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
                            User companyUser = userResponse.Result as User;

                            company.Policy.LimitCancelHours = model.LimitCancelHours;
                            company.Policy.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                            company.Policy.UpdatedBy = companyUser.UserId;

                            company.Descricao = model.Descricao;
                            company.User = companyUser;

                            company.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                            company.UpdatedBy = companyUser.UserId;

                            _companyRepository.Edit(company);

                            resp.Success = true;
                            resp.Result = "Empresa editada com sucesso";
                        }
                        else
                        {
                            resp = userResponse;
                        }
                    }
                    else
                    {
                        resp.Result = "Empresa não econtrada no banco de dados";
                    }
                }
                else
                {
                    resp.Result = validateCompany.Errors.FirstOrDefault().ToString();
                }

            }catch(Exception e)
            {
                resp.Result = "Não foi possível editar a empresa";
            }

            return resp;
        }
        public ResponseModel CreateServiceForCompany(AddServiceModel model)
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

                MeAgendaAi.Domain.Entities.Services service = new MeAgendaAi.Domain.Entities.Services
                {
                    ServiceId = Guid.NewGuid(),
                    CompanyId = company.CompanyId,
                    Name = model.Name,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                };
                _serviceRepository.Add(service);

                resp.Success = true;
                resp.Result = "Serviço adicionado à empresa com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar o serviço";
            }

            return resp;
        }

        public ResponseModel GetCompanyServices(string companyId)
        {
            var resp = new ResponseModel();

            try
            {
                var services = _serviceRepository.GetServicesByCompanyId(Guid.Parse(companyId));
                List<GetCompanyServicesModel> companyServices = new List<GetCompanyServicesModel>();
                services.ForEach(service => {
                    GetCompanyServicesModel companyService = new GetCompanyServicesModel
                    {
                        ServiceId = service.ServiceId.ToString(),
                        Name = service.Name
                    };
                    companyServices.Add(companyService);
                });
                resp.Success = true;
                resp.Result = companyServices;
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível selecionar os serviços da empresa";
            }

            return resp;
        }

        public ResponseModel UpdatePolicy(UpdatePolicyModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var company = _companyRepository.GetCompanyWithPolicyById(Guid.Parse(model.CompanyId));
                if(company == null)
                {
                    resp.Result = "Não foi posível encontrar a empresa ou a política requisitadas";
                    return resp;
                }

                Policy policy = company.Policy;
                policy.LimitCancelHours = model.LimitCancelHours;
                policy.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();

                _policyRepository.Edit(policy);

                resp.Success = true;
                resp.Result = "Atualizado com sucesso";
                return resp;
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível selecionar os serviços da empresa";
            }

            return resp;
        }

        public ResponseModel GetCompanyComplete(string companyId)
        {
            var resp = new ResponseModel();

            try
            {
                Company companyComplete = _companyRepository.GetCompanyByIdComplete(Guid.Parse(companyId));

                List<GetCompanyByIdCompleteServiceModel> companyServices = new List<GetCompanyByIdCompleteServiceModel>();
                companyComplete.Services.ForEach(service => {
                    GetCompanyByIdCompleteServiceModel companyService = new GetCompanyByIdCompleteServiceModel
                    {
                        ServiceId = service.ServiceId,
                        ServiceName = service.Name,
                        ServiceDuration = service.DurationMinutes
                    };
                    companyServices.Add(companyService);
                });

                List<GetCompanyByIdCompleteEmployeeModel> companyEmployees = new List<GetCompanyByIdCompleteEmployeeModel>();
                companyComplete.Employees.ForEach(employee => {

                    List<GetCompanyByIdCompleteServiceModel> employeeServices = new List<GetCompanyByIdCompleteServiceModel>();
                    employee.EmployeeServices.ForEach(eService => {
                        GetCompanyByIdCompleteServiceModel employeeService = new GetCompanyByIdCompleteServiceModel {
                            ServiceId = eService.Service.ServiceId,
                            ServiceDuration = eService.Service.DurationMinutes,
                            ServiceName = eService.Service.Name
                        };
                        employeeServices.Add(employeeService);
                    });

                    GetCompanyByIdCompleteEmployeeModel companyEmployee = new GetCompanyByIdCompleteEmployeeModel { 
                        EmployeeId = employee.EmployeeId,
                        EmplyeeName = employee.User.Name,
                        IsManager = employee.IsManager,
                        EmployeeServices = employeeServices
                    };
                    companyEmployees.Add(companyEmployee);
                });

                GetCompanyByIdCompleteModel model = new GetCompanyByIdCompleteModel { 
                    CompanyId = companyComplete.CompanyId,
                    //CompanyName = companyComplete.Name,
                    LimitCancelHours = companyComplete.Policy.LimitCancelHours,
                    //CPF = companyComplete.CPF,
                    CNPJ = companyComplete.CNPJ,
                    CompanyServices = companyServices,
                    Employees = companyEmployees
                };
                resp.Success = true;
                resp.Result = model;
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível selecionar os serviços da empresa";
            }

            return resp;
        }
    }
}
