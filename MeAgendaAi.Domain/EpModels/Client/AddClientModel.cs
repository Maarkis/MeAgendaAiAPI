using System.Collections.Generic;
using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using Microsoft.AspNetCore.Http;

namespace MeAgendaAi.Domain.EpModels.Client
{
    public class AddClientModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public IFormFile Imagem { get; set; }
        public List<AddLocationModel> Locations { get; set; }
        public List<AddPhoneNumberModel> PhoneNumbers { get; set; }
    }
}