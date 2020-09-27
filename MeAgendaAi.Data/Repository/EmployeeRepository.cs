using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
    }
}
