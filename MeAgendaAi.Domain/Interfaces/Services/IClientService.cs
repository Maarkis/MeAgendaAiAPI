using System.Threading.Tasks;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.EpModels;
using MeAgendaAi.Domain.EpModels.Client;
using MeAgendaAi.Domain.Interfaces.Services;

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