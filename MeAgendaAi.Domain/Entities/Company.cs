using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Entities
{
    public class Company : BaseEntity
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public virtual List<Employee> Employees { get; set; }
        public virtual List<Service> Services { get; set; }
        public virtual Policy Policy { get; set; }
    }
}
