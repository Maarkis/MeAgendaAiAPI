using System;
using MeAgendaAi.Domain.Entities;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Company GetCompanyWithPolicyById(Guid companyId);
        Company GetCompanyByIdComplete(Guid companyId);
        Company GetCompanyByUserId(Guid userId);
        string GetCompanyLink(Guid companyId);
    }
}