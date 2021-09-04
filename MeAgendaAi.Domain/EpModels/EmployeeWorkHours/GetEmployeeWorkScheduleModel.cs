namespace MeAgendaAi.Domain.EpModels.EmployeeWorkHours
{
    public class GetEmployeeWorkScheduleModel
    {
        public int Dia { get; set; }
        public string StartHour { get; set; }
        public string EndHour { get; set; }
        public string StartInterval { get; set; }
        public string EndInterval { get; set; }
    }
}