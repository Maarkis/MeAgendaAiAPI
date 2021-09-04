using System;
using System.Collections.Generic;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IServiceRepository : IBaseRepository<Entities.Services>
    {
        List<Entities.Services> GetServicesByCompanyId(Guid companyId);
    }
}