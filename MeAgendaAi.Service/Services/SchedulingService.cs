﻿using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.Scheduling;
using MeAgendaAi.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Services
{
    public class SchedulingService : BaseService<Scheduling>, ISchedulingService
    {
        private ISchedulingRepository _schedulingRepository;
        private IClientRepository _clientRepository;
        private IEmployeeRepository _employeeRepository;

        public SchedulingService(ISchedulingRepository schedulingRepository, IClientRepository clientRepository,
            IEmployeeRepository employeeRepository) : base(schedulingRepository)
        {
            _schedulingRepository = schedulingRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
        }

        public ResponseModel CreateScheduling(CreateSchedulingModel model)
        {
            var resp = new ResponseModel();

            try
            {
                Client client = _clientRepository.GetClientByUserId(Guid.Parse(model.UserId));
                if(client == null)
                {
                    resp.Result = "Não foi possível encontrar o cliente";
                    return resp;
                }

                DateTime startTime;
                if (!DateTime.TryParse(model.StartTime, out startTime))
                {
                    resp.Result = "Start time inválida";
                    return resp;
                }
                
                DateTime endTime;
                if (!DateTime.TryParse(model.EndTime, out endTime))
                {
                    resp.Result = "Start time inválida";
                    return resp;
                }

                Scheduling newScheduling = new Scheduling {
                    SchedulingId = Guid.NewGuid(),
                    EmployeeId = Guid.Parse(model.EmployeeId),
                    ClientId = client.ClientId,
                    ServiceId = Guid.Parse(model.ServiceId),
                    StartTime = startTime,
                    EndTime = endTime,
                    CreatedAt = DateTimeUtil.UtcToBrasilia(),
                    LastUpdatedAt = DateTimeUtil.UtcToBrasilia()
                };
                _schedulingRepository.Add(newScheduling);

                resp.Success = true;
                resp.Result = "Agendamento adicionado com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível realizar o agendamento";
            }

            return resp;
        }

        public ResponseModel GetClientSchedulings(string userId)
        {
            var resp = new ResponseModel();

            try
            {
                Client client = _clientRepository.GetClientByUserId(Guid.Parse(userId));
                if(client == null)
                {
                    resp.Result = "Não foi possível encontrar o cliente";
                    return resp;
                }

                var schedulings = _schedulingRepository.GetClientSchedulings(client.ClientId);
                resp.Result = SchedulingsToGetSchedulingsModel(schedulings);
                resp.Success = true;              
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível resgatar os agendamentos do usuário";
            }

            return resp;
        }

        public ResponseModel GetEmployeeSchedulings(string userId)
        {
            var resp = new ResponseModel();

            try
            {
                Employee employee = _employeeRepository.GetEmployeeByUserId(Guid.Parse(userId));
                if (employee == null)
                {
                    resp.Result = "Não foi possível encontrar o funcionário";
                    return resp;
                }

                var schedulings = _schedulingRepository.GetEmployeeSchedulings(employee.EmployeeId);
                resp.Result = SchedulingsToGetSchedulingsModel(schedulings);
                resp.Success = true;
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível resgatar os agendamentos do usuário";
            }

            return resp;
        }

        private List<GetSchedulingsModel> SchedulingsToGetSchedulingsModel(List<Scheduling> schedulings) {
            List<GetSchedulingsModel> listGetSchedulingModel = new List<GetSchedulingsModel>();

            schedulings.ForEach(scheduling => {
                GetSchedulingsModel model = new GetSchedulingsModel
                {
                    SchedulingId = scheduling.SchedulingId.ToString(),
                    ClientName = scheduling.Client.User.Name,
                    EmployeeName = scheduling.Employee.User.Name,
                    CompanyName = scheduling.Employee.Company.Name,
                    Service = scheduling.Service.Name,
                    StartTime = scheduling.StartTime.ToString(),
                    EndTime = scheduling.EndTime.ToString(),
                    Status = (int)scheduling.Status
                };
                listGetSchedulingModel.Add(model);
            });

            return listGetSchedulingModel;
        }

        public ResponseModel UpdateSchedulingStatus(UpdateSchedulingStatusModel model)
        {
            var resp = new ResponseModel();

            try
            {
                if(model.NewStatus < 0 || (int)model.NewStatus > 1)
                {
                    resp.Result = "Novo status inválido";
                    return resp;
                }

                Scheduling scheduling = _schedulingRepository.GetById(Guid.Parse(model.SchedulingId));
                if(scheduling == null)
                {
                    resp.Result = "Não foi possível encontrar o agendamento";
                    return resp;
                }

                scheduling.Status = model.NewStatus;
                scheduling.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                _schedulingRepository.Edit(scheduling);

                resp.Success = true;
                resp.Result = "Atualizado com sucesso";
            }
            catch (Exception)
            {
                resp.Result = "Não foi possível atualizar o status do agendamento";
            }

            return resp;
        }
    }
}