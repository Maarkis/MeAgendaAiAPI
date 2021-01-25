using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface ILocationRepository : IBaseRepository<Location>
    {
        List<Location> GetLocationsByUserId(Guid userId);
    }
}
