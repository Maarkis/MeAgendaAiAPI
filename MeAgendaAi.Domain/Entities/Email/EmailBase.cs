namespace MeAgendaAi.Domain.Entities.Email
{
    public class EmailBase
    {
        public EmailBase()
        {
            FromEmail = "FromEmail";
            FromName = "FromName";
            Subject = "Subject";
        }

        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Subject { get; set; }
    }
}