using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Interfaces
{
    public interface IClientService : IBaseService<Client>
    {
        ResponseModel AddClient(AddClientEpModel model);
    }
}
