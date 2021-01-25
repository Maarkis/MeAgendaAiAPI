using MeAgendaAi.Domain.EpModels.Employee;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Company
{
    public class CompanyFavInfoModel
    {
        public string CompanyId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Descricao { get; set; }
        public string Link { get; set; }
        public List<EmployeeFavInfoModel> FavoriteEmployees { get; set; }
    }
}
