using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class EmployeeWorkHours : BaseEntity
    {
        public Guid EmployeeWorkHoursId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public DateTime? StartInterval { get; set; }
        public DateTime? EndInterval { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
