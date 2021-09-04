using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities.Email
{
    public class EmailConfirmation : EmailBase
    {
        public string Url { get; set; }
    }
}
