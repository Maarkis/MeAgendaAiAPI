using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Interfaces
{
    public interface ISchedulingService : IBaseService<Scheduling>
    {
        ResponseModel CreateScheduling(CreateSchedulingModel model);
        ResponseModel GetClientSchedulings(string userId);
        ResponseModel GetEmployeeSchedulings(string userId);
        ResponseModel UpdateSchedulingStatus(UpdateSchedulingStatusModel model);
    }
}
