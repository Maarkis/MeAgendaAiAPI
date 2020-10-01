using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MeAgendaAi.Data.Repository
{
    public class SchedulingRepository : BaseRepository<Scheduling>, ISchedulingRepository
    {
        private DbSet<Scheduling> _schedulings;
        public SchedulingRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _schedulings = context.Schedulings;
        }

        public List<Scheduling> GetClientSchedulings(Guid clientId)
        {
            return _schedulings.Where(x => x.ClientId == clientId)
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .Include(x => x.Service)
                .OrderBy(x => x.StartTime)
                .ToList();
        }

        public List<Scheduling> GetEmployeeSchedulings(Guid employeeId)
        {
            return _schedulings.Where(x => x.EmployeeId == employeeId)
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .Include(x => x.Service)
                .OrderBy(x => x.StartTime)
                .ToList();
        }

        public Scheduling GetSchedulingByIdComplete(Guid schedulingId)
        {
            return _schedulings.Where(x => x.SchedulingId == schedulingId)
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .Include(x => x.Service)
                .FirstOrDefault();
        }

        public Policy GetCompanyPolicyBySchedulingId(Guid schedulingId)
        {
            return _schedulings.Where(x => x.SchedulingId == schedulingId)
                .Select(x => x.Employee)
                .Select(y => y.Company)
                .Select(z => z.Policy)
                .FirstOrDefault();
        }
    }
}
