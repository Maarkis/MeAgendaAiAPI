using System.Threading.Tasks;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Domain.Entities.Email;

namespace MeAgendaAi.Domain.Interfaces.Services.Email
{
    public interface IEmailService
    {
        Task<bool> SendRecoveryPassword(User user, EmailRetrievePassword emailRetrieve);
        Task<bool> SendEmailConfirmartion(User user, EmailConfirmation emailConfirmation);
    }
}