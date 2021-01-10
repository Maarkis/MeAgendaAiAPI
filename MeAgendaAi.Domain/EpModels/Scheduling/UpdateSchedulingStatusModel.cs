using MeAgendaAi.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Scheduling
{
    public class UpdateSchedulingStatusModel
    {
        public string SchedulingId { get; set; }
        public SchedulingStatus NewStatus { get; set; }
    }
}
