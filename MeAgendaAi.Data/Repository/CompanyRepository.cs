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

        public Company GetCompanyByIdComplete(Guid companyId)
        {
            return _companies.Where(x => x.CompanyId == companyId)
                .Include(x => x.User)
                .Include(x => x.Policy)
                .Include(x => x.Employees)
                    .ThenInclude(y => y.User)
                .Include(x => x.Employees)
                    .ThenInclude(y => y.EmployeeServices)
                    .ThenInclude(z => z.Service)
                .Include(x => x.Services)
                .FirstOrDefault();
        }

        public Company GetCompanyByUserId(Guid userId)
        {
            return _companies.Where(x => x.UserId == userId)
                .Include(x => x.User)
                .Include(x => x.Policy)
                .FirstOrDefault();
        }

        public string GetCompanyLink(Guid companyId)
        {
            return $"http://localhost:4200/perfil_empresa/{companyId}";
        }
    }
}
