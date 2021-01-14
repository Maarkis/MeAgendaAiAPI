using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.EmployeeWorkHours;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeAgendaAi.Service.Services
{
    public class EmployeeWorkHoursService : BaseService<EmployeeWorkHours>, IEmployeeWorkHoursService
    {
        private IEmployeeWorkHoursRepository _employeeWorkHoursRepository;
        private ISchedulingRepository _schedulingRepository;

        public EmployeeWorkHoursService(IEmployeeWorkHoursRepository employeeWorkHoursRepository,
            ISchedulingRepository schedulingRepository) : base(employeeWorkHoursRepository)
        {
            _employeeWorkHoursRepository = employeeWorkHoursRepository;
            _schedulingRepository = schedulingRepository;
        }

        public ResponseModel AddEmployeeWorkhours(AddEmployeeWorkHoursModel model, Employee employee)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                model.DatasComOHorario.ForEach(data => {
                    var startTime = GenerateDateTimeByTimeSpanAndDataModel(data, model.StartHour.TimeOfDay);
                    var endTime = GenerateDateTimeByTimeSpanAndDataModel(data, model.EndHour.TimeOfDay);
                    var startInterval = GenerateDateTimeByTimeSpanAndDataModel(data, model.StartInterval.TimeOfDay);
                    var endInterval = GenerateDateTimeByTimeSpanAndDataModel(data, model.EndInterval.TimeOfDay);

                    var modelWorkHours = new EmployeeWorkHours
                    {
                        EmployeeWorkHoursId = Guid.NewGuid(),
                        EmployeeId = employee.EmployeeId,
                        StartHour = startTime,
                        EndHour = endTime,
                        StartInterval = startInterval,
                        EndInterval = endInterval,
                        CreatedAt = DateTimeUtil.UtcToBrasilia(),
                        LastUpdatedAt = DateTimeUtil.UtcToBrasilia(),
                        UpdatedBy = employee.UserId
                    };

                    _employeeWorkHoursRepository.Add(modelWorkHours);
                    response.Success = true;
                    response.Result = "Adicionado com sucesso";
                });
            }catch(Exception e)
            {
                response.Result = $"Erro ao adicionar horário de trabalho.\n {e.Message}";
            }         

            return response;
        }

        public ResponseModel GetAvailableEmployeeWorkHours(DateTime date, Employee employee, Domain.Entities.Services service)
        {
            var response = new ResponseModel();

            try
            {
                var workHours = _employeeWorkHoursRepository.GetWorkHoursByDateAndEmployeeId(date, employee.EmployeeId);
                var schedulings = _schedulingRepository.GetDaySchedulingsByEmployee(employee.EmployeeId, date);

                DateTime StartTime = workHours.StartHour;
                DateTime EndTime = workHours.EndHour;
                List<DateTime> intervals = new List<DateTime>();
                while (StartTime <= EndTime.AddMinutes(service.DurationMinutes * -1))
                {
                    StartTime = StartTime.AddMinutes(service.DurationMinutes);
                    intervals.Add(StartTime);
                }

                List<DateTime> exclude = new List<DateTime>();
                intervals.ForEach(interval => {
                    // se estiver dentro do horário de intervalo
                    if ((DateTime.Compare(interval, (DateTime)workHours.StartInterval) >= 0) && (DateTime.Compare(interval, (DateTime)workHours.EndInterval) < 0))
                    {
                        exclude.Add(interval);
                    }

                    // se estiver dentro fo horário de algum agendamento do dia
                    if (schedulings.Any(x => DateTime.Compare(x.StartTime, interval) <= 0) && schedulings.Any(x => DateTime.Compare(x.EndTime, interval) > 0))
                    //if (schedulings.Any(x => x.StartTime <= interval) && schedulings.Any(x => x.EndTime > interval))
                    {
                        exclude.Add(interval);
                    }
                });

                exclude.ForEach(item => intervals.Remove(item));

                response.Success = true;
                response.Result = intervals;
            }catch(Exception e)
            {
                response.Result = "Ocorreu um erro ao calcular a lista de horários";
            }

            return response;
        }

        private DateTime GenerateDateTimeByTimeSpanAndDataModel(DateModel data, TimeSpan timeSpan)
        {
            return new DateTime(data.Ano, data.Mes, data.Dia, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
