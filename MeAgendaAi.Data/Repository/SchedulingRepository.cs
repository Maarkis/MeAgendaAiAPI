using System;
using System.Collections.Generic;
using System.Linq;
using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MeAgendaAi.Data.Repository
{
    public class SchedulingRepository : BaseRepository<Scheduling>, ISchedulingRepository
    {
        private readonly DbSet<Scheduling> _schedulings;

        public SchedulingRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context,
            configuration)
        {
            _schedulings = context.Schedulings;
        }

        public List<Scheduling> GetClientSchedulings(Guid clientId)
        {
            return _schedulings.Where(x => x.ClientId == clientId && x.Status == SchedulingStatus.Scheduled)
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .ThenInclude(y => y.User)
                .Include(x => x.Service)
                .OrderBy(x => x.StartTime)
                .ToList();
        }

        public List<Scheduling> GetClientSchedulingsProximos(Guid clientId)
        {
            return _schedulings.Where(x =>
                    x.ClientId == clientId && DateTime.Compare(x.StartTime, DateTimeUtil.UtcToBrasilia()) >= 0 &&
                    x.Status == SchedulingStatus.Scheduled)
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .ThenInclude(y => y.User)
                .Include(x => x.Service)
                .OrderBy(x => x.StartTime)
                .ToList();
        }

        public List<Scheduling> GetClientSchedulingsExpirados(Guid clientId)
        {
            return _schedulings.Where(x =>
                    x.ClientId == clientId && (DateTime.Compare(x.StartTime, DateTimeUtil.UtcToBrasilia()) < 0 ||
                                               x.Status == SchedulingStatus.Canceled))
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .ThenInclude(y => y.User)
                .Include(x => x.Service)
                .OrderByDescending(x => x.StartTime)
                .ToList();
        }

        public List<Scheduling> GetEmployeeSchedulings(Guid employeeId)
        {
            return _schedulings.Where(x => x.EmployeeId == employeeId && x.Status == SchedulingStatus.Scheduled)
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .ThenInclude(y => y.User)
                .Include(x => x.Service)
                .OrderBy(x => x.StartTime)
                .ToList();
        }

        public List<Scheduling> GetEmployeeSchedulingsProximos(Guid employeeId)
        {
            return _schedulings.Where(x =>
                    x.EmployeeId == employeeId && DateTime.Compare(x.StartTime, DateTimeUtil.UtcToBrasilia()) >= 0 &&
                    x.Status == SchedulingStatus.Scheduled)
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .ThenInclude(y => y.User)
                .Include(x => x.Service)
                .OrderBy(x => x.StartTime)
                .ToList();
        }

        public List<Scheduling> GetEmployeeSchedulingsAntigos(Guid employeeId)
        {
            return _schedulings.Where(x =>
                    x.EmployeeId == employeeId && (DateTime.Compare(x.StartTime, DateTimeUtil.UtcToBrasilia()) < 0 ||
                                                   x.Status == SchedulingStatus.Canceled))
                .Include(x => x.Client)
                .ThenInclude(y => y.User)
                .Include(x => x.Employee)
                .ThenInclude(e => e.User)
                .Include(x => x.Employee)
                .ThenInclude(x => x.Company)
                .ThenInclude(y => y.User)
                .Include(x => x.Service)
                .OrderByDescending(x => x.StartTime)
                .ToList();
        }

        public List<Scheduling> GetDaySchedulingsByEmployee(Guid employeeId, DateTime date)
        {
            return _schedulings.Where(x =>
                    x.EmployeeId == employeeId && x.StartTime.Date == date.Date &&
                    x.Status == SchedulingStatus.Scheduled)
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
                .ThenInclude(y => y.User)
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