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
    public class PhoneNumberRepository : BaseRepository<PhoneNumber>, IPhoneNumberRepository
    {
        private readonly DbSet<PhoneNumber> _phoneNumbers;

        public PhoneNumberRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context,
            configuration)
        {
            _phoneNumbers = context.PhoneNumbers;
        }

        public List<PhoneNumber> GetByUserID(Guid userId)
        {
            return _phoneNumbers.Where(x => x.UserId == userId).ToList();
        }
    }
}