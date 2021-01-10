using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Company
{
    public class GetCompanyByIdCompleteModel
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public int LimitCancelHours { get; set; }
        public List<GetCompanyByIdCompleteServiceModel> CompanyServices { get; set; }
        public List<GetCompanyByIdCompleteEmployeeModel> Employees { get; set; }
    }

    public class GetCompanyByIdCompleteEmployeeModel
    {
        public Guid EmployeeId { get; set; }
        public string EmplyeeName { get; set; }
        public bool IsManager { get; set; }
        public List<GetCompanyByIdCompleteServiceModel> EmployeeServices { get; set; }
    }

    public class GetCompanyByIdCompleteServiceModel
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int ServiceDuration { get; set; }
    }
}
