using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.User
{
    public class ResetPassword
    {
        public Guid Id { get; set; }        
        public string Token { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
