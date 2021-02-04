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
        private IEmployeeRepository _employeeRepository;

        public EmployeeWorkHoursService(IEmployeeWorkHoursRepository employeeWorkHoursRepository,
            ISchedulingRepository schedulingRepository, IEmployeeRepository employeeRepository) : base(employeeWorkHoursRepository)
        {
            _employeeWorkHoursRepository = employeeWorkHoursRepository;
            _schedulingRepository = schedulingRepository;
            _employeeRepository = employeeRepository;
        }

        public ResponseModel AddEmployeeWorkhours(AddEmployeeWorkHoursModel model, Employee employee)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                model.DatasComOHorario.ForEach(data => {

                    RemoveExistentWorkHours(data, employee.EmployeeId);

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
                    response.Message = "Horários disponíveis adicionados com sucesso";
                });
            }catch(Exception e)
            {
                response.Message = $"Erro ao adicionar horário de trabalho.\n {e.Message}";
            }         

            return response;
        }

        private void RemoveExistentWorkHours(DateModel date, Guid employeeId)
        {
            var exists = _employeeWorkHoursRepository.GetByDiaMesAno(date.Dia, date.Mes, date.Ano, employeeId);
            if (exists != null)
            {
                _employeeWorkHoursRepository.Remove(exists);
            }
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
                response.Message = "Horários selecionados com sucesso!";
            }catch(Exception e)
            {
                response.Message = "Ocorreu um erro ao calcular a lista de horários";
            }

            return response;
        }

        public ResponseModel GetWorkHoursMock(DateTime date, Employee employee, Domain.Entities.Services service)
        {
            var response = new ResponseModel();

            try
            {
                List<DateTime> intervals = new List<DateTime>();

                // Horário comercial, 09:00 - 18:00
                var workHours = new EmployeeWorkHours { 
                    EmployeeWorkHoursId = Guid.NewGuid(),
                    StartHour = new DateTime(date.Year, date.Month, date.Day, 09, 00, 00),
                    EndHour = new DateTime(date.Year, date.Month, date.Day, 18, 00, 00),
                    StartInterval = new DateTime(date.Year, date.Month, date.Day, 12, 00, 00),
                    EndInterval = new DateTime(date.Year, date.Month, date.Day, 13, 00, 00)
                };
                var schedulings = _schedulingRepository.GetDaySchedulingsByEmployee(employee.EmployeeId, date);

                DateTime StartTime = workHours.StartHour;
                DateTime EndTime = workHours.EndHour;
                
                while (StartTime <= EndTime.AddMinutes(service.DurationMinutes * -1))
                {
                    StartTime = StartTime.AddMinutes(service.DurationMinutes);
                    intervals.Add(StartTime);
                }

                List<DateTime> exclude = new List<DateTime>();
                intervals.ForEach(interval => {
                    if (DateTime.Compare(interval, DateTimeUtil.UtcToBrasilia()) < 0)
                    {
                        exclude.Add(interval);
                    }else if ((DateTime.Compare(interval, (DateTime)workHours.StartInterval) >= 0) && (DateTime.Compare(interval, (DateTime)workHours.EndInterval) < 0))
                    {
                        // se estiver dentro do horário de intervalo
                        exclude.Add(interval);
                    }else if (schedulings.Any(x => DateTime.Compare(x.StartTime, interval) <= 0) && schedulings.Any(x => DateTime.Compare(x.EndTime, interval) > 0))
                    {
                        // se estiver dentro fo horário de algum agendamento do dia
                        exclude.Add(interval);
                    }
                });

                exclude.ForEach(item => intervals.Remove(item));

                response.Success = true;
                response.Result = intervals;
                response.Message = "Horários selecionados com sucesso!";
            }
            catch (Exception e)
            {
                response.Message = "Ocorreu um erro ao calcular a lista de horários";
            }

            return response;
        }

        public ResponseModel GetEmployeeMonthSchedule(string userId, int ano, int mes)
        {
            ResponseModel resp = new ResponseModel();

            try
            {
                if (GuidUtil.IsGuidValid(userId))
                {
                    var employee = _employeeRepository.GetEmployeeByUserId(Guid.Parse(userId));
                    if(employee != null)
                    {
                        List<GetEmployeeWorkScheduleModel> scheduleModel = new List<GetEmployeeWorkScheduleModel>();
                        var workHours = _employeeWorkHoursRepository.GetByMesAno(mes, ano, employee.EmployeeId);
                        if(workHours != null && workHours.Count > 0)
                        {
                            workHours.ForEach(workHour => {

                                GetEmployeeWorkScheduleModel model = new GetEmployeeWorkScheduleModel
                                {
                                    Dia = workHour.StartHour.Day,
                                    StartHour = workHour.StartHour.ToString(),
                                    EndHour = workHour.EndHour.ToString(),
                                    StartInterval = workHour.StartInterval.ToString(),
                                    EndInterval = workHour.EndInterval.ToString()
                                };

                                scheduleModel.Add(model);

                            });
                        }

                        resp.Success = true;
                        resp.Message = "Horários do mês";
                        resp.Result = scheduleModel;
                    }
                    else
                    {
                        resp.Message = "Funcionário não encontrado";
                    }
                }
                else
                {
                    resp.Message = "Id inválido";
                }

            }catch(Exception e)
            {
                resp.Message = $"Não foi possível recuperar os horários para este mês. \n{e.Message}";
            }

            return resp;
        }

        private DateTime GenerateDateTimeByTimeSpanAndDataModel(DateModel data, TimeSpan timeSpan)
        {
            return new DateTime(data.Ano, data.Mes, data.Dia, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
