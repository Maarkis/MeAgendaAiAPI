using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Entities;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IEmployeeWorkHoursRepository : IBaseRepository<EmployeeWorkHours>
    {
        EmployeeWorkHours GetWorkHoursByDateAndEmployeeId(DateTime date, Guid employeeId);
        EmployeeWorkHours GetByDiaMesAno(int dia, int mes, int ano, Guid employeeId);
        List<EmployeeWorkHours> GetByMesAno(int mes, int ano, Guid employeeId);
    }
}