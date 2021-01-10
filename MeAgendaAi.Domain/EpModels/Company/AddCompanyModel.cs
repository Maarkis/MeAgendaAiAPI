using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Company
{
    public class AddCompanyModel
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string ManagerUserId { get; set; }
        public int LimitCancelHours { get; set; }
    }
}
