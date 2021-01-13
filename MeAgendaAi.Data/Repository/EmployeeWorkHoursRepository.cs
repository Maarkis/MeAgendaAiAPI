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
    public class EmployeeWorkHoursRepository : BaseRepository<EmployeeWorkHours>, IEmployeeWorkHoursRepository
    {
        private DbSet<EmployeeWorkHours> _employeeWorkHours;
        public EmployeeWorkHoursRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _employeeWorkHours = context.EmployeeWorkHours;
        }
    }
}
