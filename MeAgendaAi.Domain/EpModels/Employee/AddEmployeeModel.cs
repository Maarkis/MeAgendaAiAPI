using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Employee
{
    public class AddEmployeeModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public bool IsManager { get; set; }
        public string Descricao { get; set; }
        public string CompanyId { get; set; }
        public IFormFile Imagem { get; set; }
        public List<AddLocationModel> Locations { get; set; }
        public List<AddPhoneNumberModel> PhoneNumbers { get; set; }
    }
}
