using MeAgendaAi.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Utils
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params Roles[] roles) : base()
        {

            
            foreach (Roles role in roles)
            {                                
                Roles += role.ToString() + ", ";
            }
            
        }
    }
}
