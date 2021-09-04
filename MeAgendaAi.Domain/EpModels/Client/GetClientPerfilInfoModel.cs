using System.Collections.Generic;
using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;

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