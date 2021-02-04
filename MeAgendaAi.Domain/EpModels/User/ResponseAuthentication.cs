using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.User
{
    public class ResponseAuthentication
    {
        public Guid Id { get; set; }
        public string SecondaryId { get; set; }
        public bool Authenticated { get; set; }
        public string Create { get; set; }
        public string Expiration { get; set; }
        public string Image { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Message { get; set; }
        public int Role { get; set; }
    }
}
