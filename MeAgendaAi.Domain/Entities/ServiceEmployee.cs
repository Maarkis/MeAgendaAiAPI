using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class ServiceEmployee : BaseEntity
    {
        public Guid EmployeeServiceId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid EmployeeId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Service Service { get; set; }
    }
}
