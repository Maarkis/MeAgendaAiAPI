using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Company;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface ICompanyService : IBaseService<Company>
    {
        ResponseModel AddCompany(AddCompanyModel model);
        ResponseModel EditCompany(EditCompanyModel model);
        ResponseModel CreateServiceForCompany(AddServiceModel model);
        ResponseModel GetCompanyServices(string companyId);
        ResponseModel UpdatePolicy(UpdatePolicyModel model);
        ResponseModel GetCompanyInfoPerfil(string userId);
        ResponseModel GetCompanyInfo(string companyId);
        string GetCompanyLink(Guid companyId);
    }
}
