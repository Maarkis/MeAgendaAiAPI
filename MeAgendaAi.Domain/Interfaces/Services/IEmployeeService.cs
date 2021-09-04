using System;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Employee;
using MeAgendaAi.Domain.EpModels.EmployeeWorkHours;
using MeAgendaAi.Domain.Interfaces.Services;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        ResponseModel AddEmployee(AddEmployeeModel model);
        ResponseModel EditEmployee(EditEmployeeModel model);
        ResponseModel GetEmployeePerfilInfo(string userId);
        ResponseModel GetEmployeeInfo(string employeeId);
        ResponseModel GetEmployeeServices(string employeeId);
        ResponseModel AddServiceToEmployee(AddServiceToEmployeeModel model);
        ResponseModel AddWorkHoursToEmployee(AddEmployeeWorkHoursModel model, string userEmail);
        ResponseModel GetEmployeeAvailableHours(string employeeId, string serviceId, string date);
        ResponseModel GetEmployeeMonthSchedule(string userId, int ano, int mes);
        string GetEmployeeLink(Guid employeeId);
        ResponseModel GetEmployeesByServiceId(string serviceId);
        ResponseModel GetEmployeesByCompanyId(string companyId);
    }
}