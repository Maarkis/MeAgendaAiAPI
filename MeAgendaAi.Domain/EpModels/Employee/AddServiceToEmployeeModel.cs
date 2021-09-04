using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Employee
{
    public class AddServiceToEmployeeModel
    {
        public List<string> ServicesIds { get; set; }
        public string EmployeeId { get; set; }
    }
}
