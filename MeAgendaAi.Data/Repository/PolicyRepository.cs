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
    public class PolicyRepository : BaseRepository<Policy>, IPolicyRepository
    {
        private DbSet<Policy> _policies;
        public PolicyRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _policies = context.Policies;
        }
    }
}
