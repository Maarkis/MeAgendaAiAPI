using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.EpModels;
using System.Collections.Generic;
using MeAgendaAi.Domain.Enums;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface IUserService : IBaseService<User>
    {
        ResponseModel CreateUserFromModel(AddUserModel model, List<Roles> roles);
        ResponseModel Login(LoginModel model);
        ResponseModel EditUserFromModel(EditUserModel model);        
        ResponseModel ResetPassword(ResetPassword model);
        ResponseModel UserVerified(Guid id);
        Task<ResponseModel> SendEmail(RequestResendEmail model);
        ResponseModel ConfirmationEmail(Guid id);
        Task<ResponseModel> RetrievePassword(RecoveryPassword model);
        Task<ResponseModel> SendEmailConfirmation(string email);
        ResponseModel AddUserImage(AddUserImageModel model);
        Guid? GetSecondaryIdByUser(User user);
        ResponseModel EditName(RequestEditName model);
        ResponseModel Account(string id);
    }
}
