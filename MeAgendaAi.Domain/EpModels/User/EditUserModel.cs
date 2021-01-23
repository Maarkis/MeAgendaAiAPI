using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.User
{
    public class EditUserModel
    {
        public string UsuarioId { get; set; }
        public string Name { get; set; }
        public IFormFile Imagem { get; set; }
        public List<AddLocationModel> Locations { get; set; }
        public List<AddPhoneNumberModel> PhoneNumbers { get; set; }
    }
}
