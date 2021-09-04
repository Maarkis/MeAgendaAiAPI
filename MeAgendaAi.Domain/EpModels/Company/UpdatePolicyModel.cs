using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.EpModels.Company
{
    public class UpdatePolicyModel
    {
        public string CompanyId { get; set; }
        public int LimitCancelHours { get; set; }
    }
}
