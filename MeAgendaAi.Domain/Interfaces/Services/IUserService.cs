using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.EpModels;
using System.Collections.Generic;
using MeAgendaAi.Domain.Enums;
using System.Threading.Tasks;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        ResponseModel CreateUserFromModel(AddUserModel model, List<Roles> roles);
        ResponseModel Login(LoginModel model);        
        ResponseModel ResetPassword(ResetPassword model);
        Task<ResponseModel> RetrievePassword(RecoveryPassword model);
    }
}
