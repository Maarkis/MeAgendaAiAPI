using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface IClientService : IBaseService<Client>
    {
        ResponseModel AddClient(AddUserModel model);
        ResponseModel EditClient(EditUserModel model);
    }
}
