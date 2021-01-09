using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Security
{
    public class TokenConfiguration
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Seconds { get; set; }
    }
}
