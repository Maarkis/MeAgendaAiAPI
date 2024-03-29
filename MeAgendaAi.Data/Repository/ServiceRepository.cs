﻿using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeAgendaAi.Data.Repository
{
    public class ServiceRepository : BaseRepository<Services>, IServiceRepository
    {
        private DbSet<Services> _services;
        public ServiceRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _services = context.Services;
        }
        public List<Services> GetServicesByCompanyId(Guid companyId)
        {
            return _services.Where(x => x.CompanyId == companyId).ToList();
        }
    }
}
