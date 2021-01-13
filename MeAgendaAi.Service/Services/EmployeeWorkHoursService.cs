using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.EmployeeWorkHours;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Services
{
    public class EmployeeWorkHoursService : BaseService<EmployeeWorkHours>, IEmployeeWorkHoursService
    {
        private IEmployeeWorkHoursRepository _employeeWorkHoursRepository;

        public EmployeeWorkHoursService(IEmployeeWorkHoursRepository employeeWorkHoursRepository) : base(employeeWorkHoursRepository)
        {
            _employeeWorkHoursRepository = employeeWorkHoursRepository;
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

        private DateTime GenerateDateTimeByTimeSpanAndDataModel(DateModel data, TimeSpan timeSpan)
        {
            return new DateTime(data.Ano, data.Mes, data.Dia, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
