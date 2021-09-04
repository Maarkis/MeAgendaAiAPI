using System;
using System.Collections.Generic;

namespace MeAgendaAi.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public Guid EmployeeId { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Descricao { get; set; }
        public bool IsManager { get; set; }
        public virtual User User { get; set; }
        public virtual Company Company { get; set; }
        public virtual List<ServiceEmployee> EmployeeServices { get; set; }
        public virtual List<Scheduling> Schedulings { get; set; }
        public virtual List<EmployeeWorkHours> EmployeeWorkHours { get; set; }
    }
}