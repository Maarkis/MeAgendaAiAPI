using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class PhoneNumber : BaseEntity
    {
        public Guid PhoneNumberId { get; set; }
        public Guid UserId { get; set; }
        public string NameContact { get; set; }
        public int CountryCode { get; set; }
        public int DDD { get; set; }
        public string Number { get; set; }

        public virtual User User { get; set; }
    }
}
