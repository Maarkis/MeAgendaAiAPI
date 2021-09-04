using System;
using MeAgendaAi.Domain.Entities;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface IClientRepository : IBaseRepository<Client>
    {
        Client GetClientByUserId(Guid userId);
        Client GetClientByUserEmail(string userEmail);
    }
}