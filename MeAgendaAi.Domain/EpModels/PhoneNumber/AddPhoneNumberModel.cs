namespace MeAgendaAi.Domain.EpModels.PhoneNumber
{
    public class AddPhoneNumberModel
    {
        public string NameContact { get; set; }
        public int CountryCode { get; set; }
        public int DDD { get; set; }
        public string Number { get; set; }
    }
}