using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Repositories
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Company GetCompanyWithPolicyById(Guid companyId);
        Company GetCompanyByIdComplete(Guid companyId);
        Company GetCompanyByUserId(Guid userId);
    }
}
