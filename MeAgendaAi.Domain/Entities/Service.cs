using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class Service : BaseEntity
    {
        public Guid ServiceId { get; set; }
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public int DurationMinutes { get; set; }
        public virtual Company Company { get; set; }
        public virtual List<ServiceEmployee> ServiceEmployees { get; set; }
    }
}
