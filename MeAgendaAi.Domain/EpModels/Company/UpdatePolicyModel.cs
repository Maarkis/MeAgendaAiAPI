namespace MeAgendaAi.Domain.EpModels.Company
{
    public class UpdatePolicyModel
    {
        public string CompanyId { get; set; }
        public int LimitCancelHours { get; set; }
    }
}