using System;
using System.Collections.Generic;
using System.Linq;
using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeAgendaAi.Data.Repository
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        private readonly DbSet<Location> _locations;

        public LocationRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context,
            configuration)
        {
            _locations = context.Locations;
        }

        public List<Location> GetLocationsByUserId(Guid userId)
        {
            return _locations.Where(x => x.UserId == userId).ToList();
        }
    }
}