using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Interfaces.Services;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.User;
using System;
using System.Collections.Generic;
using System.Text;
using MeAgendaAi.Domain.EpModels.Client;
using System.Threading.Tasks;

namespace MeAgendaAi.Domain.Interfaces
{
    public interface IClientService : IBaseService<Client>
    {
        Task<ResponseModel> AddClient(AddClientModel model);
        ResponseModel EditClient(EditClientModel model);
        ResponseModel GetClientPerfilInfo(string userId);
        ResponseModel GetClientFavoriteEmployees(string userId);
    }
}
