using System;
using System.Collections.Generic;
using MeAgendaAi.Domain.EpModels.Location;
using MeAgendaAi.Domain.EpModels.PhoneNumber;

namespace MeAgendaAi.Domain.EpModels.Company
{
    public class GetCompanyByIdCompleteModel
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Link { get; set; }
        public string Descricao { get; set; }
        public string CNPJ { get; set; }
        public int LimitCancelHours { get; set; }
        public List<GetCompanyByIdCompleteServiceModel> CompanyServices { get; set; }
        public List<GetCompanyByIdCompleteEmployeeModel> Employees { get; set; }
        public List<LocationPerfilModel> Locations { get; set; }
        public List<PhoneNumberPerfilModel> PhoneNumbers { get; set; }
    }

    public class GetCompanyByIdCompleteEmployeeModel
    {
        public string EmployeeId { get; set; }
        public string Image { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        public string EmplyeeName { get; set; }
        public bool IsManager { get; set; }
        public List<GetCompanyByIdCompleteServiceModel> EmployeeServices { get; set; }
    }

    public class GetCompanyByIdCompleteServiceModel
    {
        public string ServiceId { get; set; }
        public string Name { get; set; }
        public int DurationMinutes { get; set; }
    }
}