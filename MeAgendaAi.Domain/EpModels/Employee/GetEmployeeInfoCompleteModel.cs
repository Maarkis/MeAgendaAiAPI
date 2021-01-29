using MeAgendaAi.Domain.EpModels.Company;
using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Employee
{
    public class GetEmployeeInfoCompleteModel
    {
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Link { get; set; }
        public string CPF { get; set; }
        public string RG { get; set; }
        public string Descricao { get; set; }
        public string DataCadastro { get; set; }
        public bool IsManager { get; set; }
        public CompanyBasicInfoModel Company { get; set; }
        public virtual List<GetCompanyByIdCompleteServiceModel> EmployeeServices { get; set; }
        public List<LocationPerfilModel> Locations { get; set; }
        public List<PhoneNumberPerfilModel> PhoneNumbers { get; set; }
    }
}
