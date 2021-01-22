using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class Client : BaseEntity
    {
        public Guid ClientId { get; set; }
        public Guid UserId { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public virtual User User { get; set; }
        public virtual List<Scheduling> Schedulings { get; set; }
    }
}
