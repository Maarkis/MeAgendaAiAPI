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
    public class ServiceEmployeeRepository : BaseRepository<ServiceEmployee>, IServiceEmployeeRepository
    {
        private DbSet<ServiceEmployee> _serviceEmployees;
        public ServiceEmployeeRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _serviceEmployees = context.ServiceEmployees;
        }
    }
}
