using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Service.EpModels;
using MeAgendaAi.Service.EpModels.User;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        ResponseModel AddUser(AddUserModel model);
        ResponseModel LoginMock();
    }
}
