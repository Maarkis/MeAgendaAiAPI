using MeAgendaAi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.EpModels.Scheduling
{
    public class GetSchedulingsModel
    {
        public string SchedulingId { get; set; }
        public string ClientName { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public string Service { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int Status { get; set; }
    }
}
