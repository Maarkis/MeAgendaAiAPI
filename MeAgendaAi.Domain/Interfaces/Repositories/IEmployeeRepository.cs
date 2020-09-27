using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IEmployeeRepository : IBaseRepository<Employee>
    {
        List<Service> GetEmployeeServicesByEmployeeId(Guid employeeId);
    }
}
