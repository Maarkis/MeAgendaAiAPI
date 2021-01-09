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
        public string CPF { get; set; }
        public string RG { get; set; }

        //public string Roles { get; set; }
    }
}
