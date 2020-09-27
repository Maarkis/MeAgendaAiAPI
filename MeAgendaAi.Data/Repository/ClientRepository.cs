using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Repository
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        private DbSet<Client> _clients;
        public ClientRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _clients = context.Clients;
        }

        public Client GetClientByUserId(Guid userId) {
            return _clients.Where(x => x.UserId == userId).FirstOrDefault();
        }
    }
}
