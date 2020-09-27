using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface ISchedulingRepository : IBaseRepository<Scheduling>
    {
        List<Scheduling> GetClientSchedulings(Guid clientId);
        List<Scheduling> GetEmployeeSchedulings(Guid employeeId);
    }
}
