using MeAgendaAi.Data.Context;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;

namespace MeAgendaAi.Data.Repository
{
    public class EmployeeWorkHoursRepository : BaseRepository<EmployeeWorkHours>, IEmployeeWorkHoursRepository
    {
        private DbSet<EmployeeWorkHours> _employeeWorkHours;
        public EmployeeWorkHoursRepository(MeAgendaAiContext context, IConfiguration configuration) : base(context, configuration)
        {
            _employeeWorkHours = context.EmployeeWorkHours;
        }

        public EmployeeWorkHours GetWorkHoursByDateAndEmployeeId(DateTime date, Guid employeeId)
        {
            return _employeeWorkHours.Where(x => x.StartHour.Date == date.Date && x.EmployeeId == employeeId).FirstOrDefault();
        }

        public EmployeeWorkHours GetByDiaMesAno(int dia, int mes, int ano, Guid employeeId)
        {
            return _employeeWorkHours.Where(x => x.EmployeeId == employeeId && x.StartHour.Day == dia && x.StartHour.Month == mes && x.StartHour.Year == ano).FirstOrDefault();
        }

        public List<EmployeeWorkHours> GetByMesAno(int mes, int ano, Guid employeeId)
        {
            return _employeeWorkHours.Where(x => x.EmployeeId == employeeId &&  x.StartHour.Month == mes && x.StartHour.Year == ano).OrderBy(x => x.StartHour.Day).ToList();
        }
    }
}
