using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private DbSet<User> _users;
        public UserRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _users = context.Users;
        }

        public User GetByEmail(string email)
        {
            return _users.Where(x => x.Email == email).Include(x => x.Roles).FirstOrDefault();
        }

        public User GetAccountById(Guid id)
        {
            return _users.Where(x => x.UserId == id)
                .Include(x => x.Roles)
                .Include(x => x.PhoneNumbers).Include(x => x.Locations)
                .FirstOrDefault();
        }
    }
}
