using System;

namespace MeAgendaAi.Domain.EpModels.User
{
    public class RequestEditName
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}