using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.Entities;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IPhoneNumberRepository : IBaseRepository<PhoneNumber>
    {
        List<PhoneNumber> GetByUserID(Guid userId);
    }
}