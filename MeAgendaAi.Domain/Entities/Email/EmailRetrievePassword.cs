using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities.Email
{
    public class EmailRetrievePassword : EmailBase
    {
        public string Url { get; set; }
        public string Token { get; set; }
        public string Expiration { get; set; }
    }
}
