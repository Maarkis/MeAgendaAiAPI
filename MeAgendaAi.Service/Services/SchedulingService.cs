using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Enums;
using MeAgendaAi.Domain.Interfaces;
using MeAgendaAi.Domain.Interfaces.Repositories;
using MeAgendaAi.Domain.Utils;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Scheduling;
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
        private IEmployeeService _employeeService;
        private ICompanyService _companyService;
        private IServiceRepository _serviceRepository;

        public SchedulingService(ISchedulingRepository schedulingRepository, IClientRepository clientRepository,
            IEmployeeRepository employeeRepository, ICompanyService companyService, IServiceRepository serviceRepository,
            IEmployeeService employeeService) : base(schedulingRepository)
        {
            _schedulingRepository = schedulingRepository;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _companyService = companyService;
            _serviceRepository = serviceRepository;
            _employeeService = employeeService;
        }

        public ResponseModel CreateScheduling(CreateSchedulingModel model)
        {
            var resp = new ResponseModel();

            try
            {
                Client client = _clientRepository.GetClientByUserId(Guid.Parse(model.UserId));
                if(client == null)
                {
                    resp.Message = "Não foi possível encontrar o cliente";
                    return resp;
                }

                Employee employee = _employeeRepository.GetById(Guid.Parse(model.EmployeeId));
                if(employee == null)
                {
                    resp.Message = "Não foi possível encontrar o funciónário";
                    return resp;
                }

                Domain.Entities.Services service = _serviceRepository.GetById(Guid.Parse(model.ServiceId));
                if(service == null)
                {
                    resp.Message = "Não foi possível encontrar o serviço";
                    return resp;
                }

                DateTime startTime;
                if (!DateTime.TryParse(model.StartTime, out startTime))
                {
                    resp.Message = "Start time inválida";
                    return resp;
                }
                
                DateTime endTime;
                if (!DateTime.TryParse(model.EndTime, out endTime))
                {
                    resp.Message = "End time inválida";
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

                Scheduling scheduling = _schedulingRepository.GetSchedulingByIdComplete(newScheduling.SchedulingId);
                if(scheduling == null)
                {
                    resp.Message = "Falha ao realizar o agendamento";
                    return resp;
                }
                resp.Success = true;
                resp.Message = "Agendamento realizado com sucesso!";
                resp.Result = SchedulingToGetSchedulingModel(scheduling);
            }
            catch (Exception e)
            {
                resp.Message = $"Não foi possível realizar o agendamento. {e.Message}";
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
                    resp.Message = "Não foi possível encontrar o cliente";
                    return resp;
                }

                List<Scheduling> schedulings = _schedulingRepository.GetClientSchedulingsProximos(client.ClientId);
                resp.Result = SchedulingsToGetSchedulingsModel(schedulings);
                resp.Message = "Agendamentos do cliente selecionados com sucesso!";
                resp.Success = true;              
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível resgatar os agendamentos do usuário";
            }

            return resp;
        }

        public ResponseModel GetHistoricoClientSchedulings(string userId)
        {
            var resp = new ResponseModel();

            try
            {
                Client client = _clientRepository.GetClientByUserId(Guid.Parse(userId));
                if (client == null)
                {
                    resp.Message = "Não foi possível encontrar o cliente";
                    return resp;
                }

                List<Scheduling> schedulings = _schedulingRepository.GetClientSchedulingsExpirados(client.ClientId);
                resp.Result = SchedulingsToGetSchedulingsModel(schedulings);
                resp.Message = "Agendamentos do cliente selecionados com sucesso!";
                resp.Success = true;
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível resgatar os agendamentos do usuário";
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
                    resp.Message = "Não foi possível encontrar o funcionário";
                    return resp;
                }

                var schedulings = _schedulingRepository.GetEmployeeSchedulingsProximos(employee.EmployeeId);
                resp.Result = SchedulingsToGetSchedulingsModel(schedulings);
                resp.Message = "Agendamentos do Funcionário selecionados com sucesso!";
                resp.Success = true;
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível resgatar os agendamentos do usuário";
            }

            return resp;
        }

        public ResponseModel GetHistoricoEmployeeSchedulings(string userId)
        {
            var resp = new ResponseModel();

            try
            {
                Employee employee = _employeeRepository.GetEmployeeByUserId(Guid.Parse(userId));
                if (employee == null)
                {
                    resp.Message = "Não foi possível encontrar o funcionário";
                    return resp;
                }

                var schedulings = _schedulingRepository.GetEmployeeSchedulingsAntigos(employee.EmployeeId);
                resp.Result = SchedulingsToGetSchedulingsModel(schedulings);
                resp.Message = "Agendamentos do Funcionário selecionados com sucesso!";
                resp.Success = true;
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível resgatar os agendamentos do usuário";
            }

            return resp;
        }

        private List<GetSchedulingsModel> SchedulingsToGetSchedulingsModel(List<Scheduling> schedulings) {
            List<GetSchedulingsModel> listGetSchedulingModel = new List<GetSchedulingsModel>();

            schedulings.ForEach(scheduling => {
                GetSchedulingsModel model = SchedulingToGetSchedulingModel(scheduling);
                listGetSchedulingModel.Add(model);
                //if (scheduling.Status != SchedulingStatus.Canceled) {

                //}
            });

            return listGetSchedulingModel;
        }

        private GetSchedulingsModel SchedulingToGetSchedulingModel(Scheduling scheduling)
        {
            GetSchedulingsModel model = new GetSchedulingsModel
            {
                SchedulingId = scheduling.SchedulingId.ToString(),
                ClientName = scheduling.Client.User.Name,
                EmployeeName = scheduling.Employee.User.Name,
                CompanyName = scheduling.Employee.Company.User.Name,
                CompanyLink = _companyService.GetCompanyLink(scheduling.Employee.CompanyId),
                EmployeeLink = _employeeService.GetEmployeeLink(scheduling.EmployeeId),
                Service = scheduling.Service.Name,
                StartTime = scheduling.StartTime,
                EndTime = scheduling.EndTime,
                Status = (int)scheduling.Status
            };
            return model;
        }

        public ResponseModel UpdateSchedulingStatus(UpdateSchedulingStatusModel model)
        {
            var resp = new ResponseModel();

            try
            {
                if(model.NewStatus < 0 || (int)model.NewStatus > 1)
                {
                    resp.Message = "Novo status inválido";
                    return resp;
                }

                Scheduling scheduling = _schedulingRepository.GetById(Guid.Parse(model.SchedulingId));
                if(scheduling == null)
                {
                    resp.Message = "Não foi possível encontrar o agendamento";
                    return resp;
                }

                if(model.NewStatus == SchedulingStatus.Canceled)
                {
                    Policy policy = _schedulingRepository.GetCompanyPolicyBySchedulingId(Guid.Parse(model.SchedulingId));
                    DateTime now = DateTimeUtil.UtcToBrasilia();
                    DateTime limitCancel = scheduling.StartTime.AddHours(-(policy.LimitCancelHours));
                    int compare = DateTime.Compare(now, limitCancel);
                    if (compare >= 0)
                    {
                        resp.Message = "Não é possível realizar o cancelamento." +
                            "O Horário limite para cancelamento era: " + limitCancel;
                        return resp;
                    }
                }

                scheduling.Status = model.NewStatus;
                scheduling.LastUpdatedAt = DateTimeUtil.UtcToBrasilia();
                _schedulingRepository.Edit(scheduling);

                resp.Success = true;
                resp.Message = "Atualizado com sucesso";
            }
            catch (Exception)
            {
                resp.Message = "Não foi possível atualizar o status do agendamento";
            }

            return resp;
        }
    }
}
