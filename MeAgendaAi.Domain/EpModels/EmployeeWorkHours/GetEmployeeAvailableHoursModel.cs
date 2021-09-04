using System;

namespace MeAgendaAi.Domain.EpModels.EmployeeWorkHours
{
    public class GetEmployeeAvailableHoursModel
    {
        public string EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public string ServiceId { get; set; }
    }
}