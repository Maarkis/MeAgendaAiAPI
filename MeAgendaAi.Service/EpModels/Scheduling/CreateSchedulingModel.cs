﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.EpModels.Scheduling
{
    public class CreateSchedulingModel
    {
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public string ServiceId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
