using MeAgendaAi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace MeAgendaAi.Domain.Utils
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Roles[] roles)
        {
            foreach (var role in roles) Roles += role + ", ";
        }
    }
}