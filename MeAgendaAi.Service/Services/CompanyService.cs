using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.Company;
using MeAgendaAi.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Services
{
    public class CompanyService : BaseService<Company>, ICompanyService
    {
        private ICompanyRepository _companyRepository;
        private IUserRepository _userRepository;
        private IEmployeeRepository _employeeRepository;
        private IServiceRepository _serviceRepository;
        private IPolicyRepository _policyRepository;
        public CompanyService(ICompanyRepository companyRepository, IUserRepository userRepository,
            IEmployeeRepository employeeRepository, IServiceRepository serviceRepository,
            IPolicyRepository policyRepository) : base(companyRepository)
        {
            _companyRepository = companyRepository;
            _userRepository = userRepository;
            _employeeRepository = employeeRepository;
            _serviceRepository = serviceRepository;
            _policyRepository = policyRepository;
        }

        public ResponseModel AddCompany(AddCompanyModel model)
        {
            var resp = new ResponseModel();

            try
            {
                var userManager = _userRepository.GetById(Guid.Parse(model.ManagerUserId));
                if(userManager == null)
                {
                    resp.Result = "Não foi possível encontrar o usuário";
                    return resp;
                }

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
                    Name = model.Name,
                    CPF = model.CPF,
                    CNPJ = model.CNPJ,
                    Policy = policy,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                };
                _companyRepository.Add(newCompany);
                Employee employee = new Employee
                {
                    UserId = userManager.UserId,
                    CompanyId = newCompany.CompanyId,
                    IsManager = true,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                };
                _employeeRepository.Add(employee);

                resp.Success = true;
                resp.Result = "Empresa adicionada com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar a empresa";
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

                Domain.Entities.Service service = new Domain.Entities.Service
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
                    CompanyName = companyComplete.Name,
                    LimitCancelHours = companyComplete.Policy.LimitCancelHours,
                    CPF = companyComplete.CPF,
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
