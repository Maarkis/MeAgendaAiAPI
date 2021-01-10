using MeAgendaAi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class UserRole : BaseEntity
    {
        public Guid UserRoleId { get; set; }
        public Guid UserId { get; set; }
        public Roles Role { get; set; }

        public virtual User User { get; set; }
    }
}
