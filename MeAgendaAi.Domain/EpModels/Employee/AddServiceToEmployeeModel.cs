using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Employee
{
    public class AddServiceToEmployeeModel
    {
        public string ServiceId { get; set; }
        public string EmployeeId { get; set; }
    }
}
