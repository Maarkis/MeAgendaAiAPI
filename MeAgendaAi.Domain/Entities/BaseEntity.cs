using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime _createdAt { get; set; }
        public DateTime CreatedAt { 
            get { return _createdAt; }
            set { _createdAt = (value == null ? DateTime.UtcNow : value); } 
        }
        public DateTime UpdatedAt { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
