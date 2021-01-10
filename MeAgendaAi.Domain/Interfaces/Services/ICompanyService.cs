using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.Company;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface ICompanyService : IBaseService<Company>
    {
        ResponseModel AddCompany(AddCompanyModel model);
        ResponseModel CreateServiceForCompany(AddServiceModel model);
        ResponseModel GetCompanyServices(string companyId);
        ResponseModel UpdatePolicy(UpdatePolicyModel model);
        ResponseModel GetCompanyComplete(string companyId);
    }
}
