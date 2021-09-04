namespace MeAgendaAi.Domain.EpModels.Company
{
    public class GetCompanyServicesModel
    {
        public string ServiceId { get; set; }
        public string Name { get; set; }
        public int DurationMinutes { get; set; }
    }
}