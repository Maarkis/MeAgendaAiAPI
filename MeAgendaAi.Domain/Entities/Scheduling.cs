using System;
using MeAgendaAi.Domain.Enums;

namespace MeAgendaAi.Domain.Entities
{
    public class Scheduling : BaseEntity
    {
        public Guid SchedulingId { get; set; }
        public Guid ClientId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public SchedulingStatus Status { get; set; }

        public virtual Client Client { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Services Service { get; set; }
    }
}