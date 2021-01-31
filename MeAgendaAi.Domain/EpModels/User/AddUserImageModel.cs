using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.User
{
    public class AddUserImageModel
    {
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
    }
}
