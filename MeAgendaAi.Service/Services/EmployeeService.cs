using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.Employee;
using MeAgendaAi.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        private IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
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
    }
}
