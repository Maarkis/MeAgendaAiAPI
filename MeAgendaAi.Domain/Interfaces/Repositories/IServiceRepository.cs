using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IServiceRepository : IBaseRepository<Service>
    {
        List<Service> GetServicesByCompanyId(Guid companyId);
    }
}
