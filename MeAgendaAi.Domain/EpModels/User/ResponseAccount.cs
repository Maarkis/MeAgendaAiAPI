using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.User
{
    public class ResponseAccount
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public DateTime CreateAt { get; set; }
        public  List<Entities.Location> Locations { get; set; }
        public  List<Entities.PhoneNumber> PhoneNumbers { get; set; }
        public  List<Entities.UserRole> Roles { get; set; }
  

    }
}
