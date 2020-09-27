using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Data.Repository
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
    {
        private DbSet<Company> _companies;
        public CompanyRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _companies = context.Companies;
        }

        public Company GetCompanyWithPolicyById(Guid companyId) {
            return _companies.Where(x => x.CompanyId == companyId).Include(x => x.Policy).FirstOrDefault();
        }
    }
}
