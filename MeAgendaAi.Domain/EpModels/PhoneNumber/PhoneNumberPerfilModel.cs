using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.PhoneNumber
{
    public class PhoneNumberPerfilModel
    {
        public string PhoneNumberId { get; set; }
        public string NameContact { get; set; }
        public string CompletePhoneNumber { get; set; }
        public int CountryCode { get; set; }
        public int DDD { get; set; }
        public string Number { get; set; }
    }
}
