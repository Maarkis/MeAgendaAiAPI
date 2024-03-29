﻿using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface ISchedulingService : IBaseService<Scheduling>
    {
        ResponseModel CreateScheduling(CreateSchedulingModel model);
        ResponseModel GetClientSchedulings(string userId);
        ResponseModel GetHistoricoClientSchedulings(string userId);
        ResponseModel GetEmployeeSchedulings(string userId);
        ResponseModel GetHistoricoEmployeeSchedulings(string userId);
        ResponseModel UpdateSchedulingStatus(UpdateSchedulingStatusModel model);
    }
}
