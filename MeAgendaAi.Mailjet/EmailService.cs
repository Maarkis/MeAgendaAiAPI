using Mailjet.Client;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MeAgendaAi.Mailjet
{
    public class EmailService
    {
        private readonly IMailjetClient _mailjetClient;
        public EmailService(IMailjetClient mailjetClient)
        {
            _mailjetClient = mailjetClient;
            
        }

    }
}
