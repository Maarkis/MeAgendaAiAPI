using System.Collections.Generic;
using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using Microsoft.AspNetCore.Http;

namespace MeAgendaAi.Domain.EpModels.Company
{
    public class AddCompanyModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string CNPJ { get; set; }
        public string Descricao { get; set; }
        public int LimitCancelHours { get; set; }
        public IFormFile Imagem { get; set; }
        public List<AddLocationModel> Locations { get; set; }
        public List<AddPhoneNumberModel> PhoneNumbers { get; set; }
    }
}