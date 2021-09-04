using System.Collections.Generic;
using MeAgendaAi.Domain.EpModels.Company;

namespace MeAgendaAi.Domain.EpModels.Employee
{
    public class EmployeeFavInfoModel
    {
        public string EmployeeId { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Descricao { get; set; }
        public string Image { get; set; }
        public List<EmployeeFavServiceModel> Services { get; set; }
        public CompanyFavInfoModel Company { get; set; }
    }

    public class EmployeeFavServiceModel
    {
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
    }
}