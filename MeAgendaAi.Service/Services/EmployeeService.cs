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

namespace MeAgendaAi.Service.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;
        private IServiceEmployeeRepository _serviceEmployeeRepository;
        private IEmployeeWorkHoursService _employeeWorkHoursService;

        public EmployeeService(IEmployeeRepository employeeRepository, 
            IServiceEmployeeRepository serviceEmployeeRepository,
            IEmployeeWorkHoursService employeeWorkHoursService) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _serviceEmployeeRepository = serviceEmployeeRepository;
            _employeeWorkHoursService = employeeWorkHoursService;
        }

        public ResponseModel AddEmployee(AddEmployeeModel model)
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
                    RG = model.RG,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                };
                Employee newEmployee = new Employee
                {
                    EmployeeId = Guid.NewGuid(),
                    CompanyId = Guid.Parse(model.CompanyId),
                    IsManager = model.IsManager,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                    User = newUser
                };
                _employeeRepository.Add(newEmployee);

                resp.Success = true;
                resp.Result = "Funcionário adicionado com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar o funcionário";
            }

            return resp;
        }

        public ResponseModel AddServiceToEmployee(AddServiceToEmployeeModel model)
        {
            var resp = new ResponseModel();

            try
            {
                ServiceEmployee serviceEmployee = new ServiceEmployee
                {
                    EmployeeServiceId = Guid.NewGuid(),
                    ServiceId = Guid.Parse(model.ServiceId),
                    EmployeeId = Guid.Parse(model.EmployeeId),
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                };
                _serviceEmployeeRepository.Add(serviceEmployee);
                resp.Success = true;
                resp.Result = "Serviço adicionado ao funcionário com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar o serviço ao funcionário";
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
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível adicionar o serviço ao funcionário";
            }

            return resp;
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
                response.Result = "Funcionário não encontrado";
            }

            return response;
        }
    }
}
