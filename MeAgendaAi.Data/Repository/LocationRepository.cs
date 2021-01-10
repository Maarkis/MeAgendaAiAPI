using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Repository
{
    public class LocationRepository : BaseRepository<Location>, ILocationRepository
    {
        private DbSet<Location> _locations;
        public LocationRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _locations = context.Locations;
        }
    }
}
