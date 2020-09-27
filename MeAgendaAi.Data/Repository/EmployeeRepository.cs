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
        public List<Service> GetEmployeeServicesByEmployeeId(Guid employeeId)
        {
            return _employees.Where(x => x.EmployeeId == employeeId)
                .SelectMany(y => y.EmployeeServices)
                .Select(y => y.Service).ToList();
        }
    }
}
