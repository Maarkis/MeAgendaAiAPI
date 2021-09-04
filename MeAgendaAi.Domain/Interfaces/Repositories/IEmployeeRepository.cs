using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        List<MeAgendaAi.Domain.Entities.Services> GetEmployeeServicesByEmployeeId(Guid employeeId);
        Employee GetEmployeeByUserId(Guid userId);
        Employee GetEmployeeByUserEmail(string userEmail);
        Employee GetByIdComplete(Guid employeeId);
        List<Employee> GetEmployeesByClientId(Guid clientId);
        List<Employee> GetEmployeesByServiceId(Guid serviceId);
        List<Employee> GetEmployeesByCompanyId(Guid companyId);
        Employee GetEmployeeByIdWithServices(Guid employeeId);
        string GetEmployeeLink(Guid employeeId);
    }
}
