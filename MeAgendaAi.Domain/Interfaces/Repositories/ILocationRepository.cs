using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Entities;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface ILocationRepository : IBaseRepository<Location>
    {
        List<Location> GetLocationsByUserId(Guid userId);
    }
}