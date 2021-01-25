
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.EmployeeWorkHours;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Services
{
    public interface IEmployeeWorkHoursService : IBaseService<EmployeeWorkHours>
    {
        ResponseModel AddEmployeeWorkhours(AddEmployeeWorkHoursModel model, Employee employee);
        ResponseModel GetAvailableEmployeeWorkHours(DateTime date, Employee employee, Domain.Entities.Services service);
        ResponseModel GetEmployeeMonthSchedule(string userId, int ano, int mes);
    }
}
