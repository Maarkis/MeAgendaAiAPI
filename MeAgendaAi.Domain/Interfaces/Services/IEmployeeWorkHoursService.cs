using System;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.EmployeeWorkHours;

namespace MeAgendaAi.Domain.Interfaces.Services
{
    public interface IEmployeeWorkHoursService : IBaseService<EmployeeWorkHours>
    {
        ResponseModel AddEmployeeWorkhours(AddEmployeeWorkHoursModel model, Employee employee);
        ResponseModel GetAvailableEmployeeWorkHours(DateTime date, Employee employee, Entities.Services service);
        ResponseModel GetEmployeeMonthSchedule(string userId, int ano, int mes);
        ResponseModel GetWorkHoursMock(DateTime date, Employee employee, Entities.Services service);
    }
}