using System;

namespace MeAgendaAi.Domain.Entities
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}