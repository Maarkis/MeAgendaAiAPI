using System;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Company;
using MeAgendaAi.Domain.Interfaces.Services;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface ICompanyService : IBaseService<Company>
    {
        ResponseModel AddCompany(AddCompanyModel model);
        ResponseModel EditCompany(EditCompanyModel model);
        ResponseModel CreateServiceForCompany(AddMultipleServicesModel model);
        ResponseModel GetCompanyServices(string companyId);
        ResponseModel UpdatePolicy(UpdatePolicyModel model);
        ResponseModel GetCompanyInfoPerfil(string userId);
        ResponseModel GetCompanyInfo(string companyId);
        string GetCompanyLink(Guid companyId);
    }
}