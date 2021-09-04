using System;

namespace MeAgendaAi.Domain.EpModels.Scheduling
{
    public class GetSchedulingsModel
    {
        public string SchedulingId { get; set; }
        public string ClientName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeLink { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLink { get; set; }
        public string Service { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int Status { get; set; }
    }
}