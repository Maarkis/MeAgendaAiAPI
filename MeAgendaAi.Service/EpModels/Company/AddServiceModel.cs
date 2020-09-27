using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.EpModels.Company
{
    public class AddServiceModel
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public int DurationMinutes { get; set; }
    }
}
