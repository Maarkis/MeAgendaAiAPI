using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool Verified { get; set; }
        public virtual List<Location> Locations { get; set; }
        public virtual List<PhoneNumber> PhoneNumbers { get; set; }
        public virtual List<UserRole> Roles { get; set; }
    }
}
