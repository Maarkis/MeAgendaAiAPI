using System;
using MeAgendaAi.Domain.Enums;

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