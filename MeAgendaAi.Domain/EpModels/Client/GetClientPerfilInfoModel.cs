using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Client
{
    public class GetClientPerfilInfoModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Imagem { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string DataCadastro { get; set; }
        public List<LocationPerfilModel> Locations { get; set; }
        public List<PhoneNumberPerfilModel> PhoneNumbers { get; set; }
    }
}
