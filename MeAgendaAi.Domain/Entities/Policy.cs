using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class Policy : BaseEntity
    {
        public Guid PolicyId { get; set; }
        public Guid CompanyId { get; set; }
        public int LimitCancelHours { get; set; }
        public virtual Company Company { get; set; }
    }
}
