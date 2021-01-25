using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeAgendaAi.Data.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        private DbSet<Employee> _employees;
        public EmployeeRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _employees = context.Employees;
        }

        public Employee GetEmployeeByUserId(Guid userId)
        {
            return _employees.Where(x => x.UserId == userId).FirstOrDefault();
        }
        public List<Services> GetEmployeeServicesByEmployeeId(Guid employeeId)
        {
            return _employees.Where(x => x.EmployeeId == employeeId)
                .SelectMany(y => y.EmployeeServices)
                .Select(y => y.Service).ToList();
        }

        public Employee GetEmployeeByUserEmail(string userEmail)
        {
            return _employees.Where(x => x.User.Email == userEmail)
                .Include(x => x.User)
                .FirstOrDefault();
        }

        public Employee GetByIdComplete(Guid employeeId)
        {
            return _employees.Where(x => x.EmployeeId == employeeId)
                .Include(x => x.User)
                .Include(x => x.EmployeeServices)
                .ThenInclude(y => y.Service)
                .Include(x => x.Company)
                .ThenInclude(y => y.User)
                .FirstOrDefault();
        }

        public List<Employee> GetEmployeesByClientId(Guid clientId)
        {
            return _employees.Where(x => x.Schedulings.Any(y => y.ClientId == clientId))
                .Include(x => x.User)
                .Include(x => x.EmployeeServices)
                .ThenInclude(y => y.Service)
                .Include(x => x.Company)
                .ThenInclude(y => y.User)
                .ToList();
        }

    }
}
