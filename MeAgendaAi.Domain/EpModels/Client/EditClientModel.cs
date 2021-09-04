using System.Collections.Generic;
using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using Microsoft.AspNetCore.Http;

namespace MeAgendaAi.Domain.EpModels.Client
{
    public class EditClientModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string RG { get; set; }
        public IFormFile Imagem { get; set; }
        public List<AddLocationModel> Locations { get; set; }
        public List<AddPhoneNumberModel> PhoneNumbers { get; set; }
    }
}