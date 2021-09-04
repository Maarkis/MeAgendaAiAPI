using FluentValidation;
using MeAgendaAi.Domain.EpModels.User;

namespace MeAgendaAi.Domain.Validators.Authentication
{
    public class RecoveryEmailValidator : AbstractValidator<RecoveryPassword>
    {
        public RecoveryEmailValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("E-mail obrigátorio.")
                .EmailAddress()
                .WithMessage("Digite um e-mail válido.");
        }
    }
}