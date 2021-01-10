using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IServiceRepository : IBaseRepository<MeAgendaAi.Domain.Entities.Services>
    {
        List<MeAgendaAi.Domain.Entities.Services> GetServicesByCompanyId(Guid companyId);
    }
}
