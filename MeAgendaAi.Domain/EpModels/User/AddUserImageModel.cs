using Microsoft.AspNetCore.Http;

namespace MeAgendaAi.Domain.EpModels.User
{
    public class AddUserImageModel
    {
        public IFormFile Image { get; set; }
        public string UserId { get; set; }
    }
}