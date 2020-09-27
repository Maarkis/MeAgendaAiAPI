using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.EpModels.Company
{
    public class AddCompanyModel
    {
        public string Name { get; set; }
        public string CPF { get; set; }
        public string CNPJ { get; set; }
        public string ManagerUserId { get; set; }
    }
}
