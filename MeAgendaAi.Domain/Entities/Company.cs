using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class Company : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public Guid UserId { get; set; }
        public string CNPJ { get; set; }
        public string Descricao { get; set; }
        public virtual User User { get; set; }
        public virtual List<Employee> Employees { get; set; }
        public virtual List<Services> Services { get; set; }
        public virtual Policy Policy { get; set; }
        public virtual List<Location> Locations { get; set; }
    }
}
