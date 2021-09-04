using MeAgendaAi.Domain.Enums;

namespace MeAgendaAi.Domain.EpModels.Scheduling
{
    public class UpdateSchedulingStatusModel
    {
        public string SchedulingId { get; set; }
        public SchedulingStatus NewStatus { get; set; }
    }
}