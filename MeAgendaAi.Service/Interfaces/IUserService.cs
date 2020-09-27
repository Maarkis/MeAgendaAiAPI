using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Interfaces.Services
{
    public interface IUserService : IBaseService<User>
    {
        ResponseModel AddUser(AddUserModel model);
        ResponseModel LoginMock();
    }
}
