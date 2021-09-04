using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.EmployeeWorkHours
{
    public class AddEmployeeWorkHoursModel
    {
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public DateTime StartInterval { get; set; }
        public DateTime EndInterval { get; set; }
        public List<DateModel> DatasComOHorario { get; set; }
    }

    public class DateModel
    {
        public int Ano { get; set; }
        public int Mes { get; set; }
        public int Dia { get; set; }
    }
}
