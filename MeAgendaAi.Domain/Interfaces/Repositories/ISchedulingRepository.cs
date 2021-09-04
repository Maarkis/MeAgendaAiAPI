using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Entities;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface ISchedulingRepository : IBaseRepository<Scheduling>
    {
        List<Scheduling> GetClientSchedulings(Guid clientId);
        List<Scheduling> GetEmployeeSchedulings(Guid employeeId);
        List<Scheduling> GetClientSchedulingsProximos(Guid clientId);
        List<Scheduling> GetClientSchedulingsExpirados(Guid clientId);
        List<Scheduling> GetDaySchedulingsByEmployee(Guid employeeId, DateTime date);
        List<Scheduling> GetEmployeeSchedulingsProximos(Guid employeeId);
        List<Scheduling> GetEmployeeSchedulingsAntigos(Guid employeeId);
        Policy GetCompanyPolicyBySchedulingId(Guid schedulingId);
        Scheduling GetSchedulingByIdComplete(Guid schedulingId);
    }
}