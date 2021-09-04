using System.Collections.Generic;

namespace MeAgendaAi.Domain.EpModels.Company
{
    public class AddMultipleServicesModel
    {
        public string CompanyId { get; set; }
        public List<AddServiceModel> Services { get; set; }
    }

    public class AddServiceModel
    {
        public string Name { get; set; }
        public int DurationMinutes { get; set; }
    }
}