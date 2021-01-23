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
    public class PhoneNumberRepository : BaseRepository<PhoneNumber>, IPhoneNumberRepository
    {
        private DbSet<PhoneNumber> _phoneNumbers;
        public PhoneNumberRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _phoneNumbers = context.PhoneNumbers;
        }
    }
}
