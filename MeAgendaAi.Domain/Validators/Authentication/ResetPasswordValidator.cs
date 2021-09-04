using FluentValidation;
using MeAgendaAi.Domain.EpModels.User;

namespace MeAgendaAi.Domain.Validators.Authentication
{
    public class ResetPasswordValidator : AbstractValidator<ResetPassword>
    {
        public ResetPasswordValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Usuário não encontrado.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Digite uma senha.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirme a nova senha.")
                .Equal(x => x.Password)
                .WithMessage("Senha de confirmação deve ser igual a nova senha");

            RuleFor(x => x.Token)
                .NotEmpty().WithMessage("Token não encontrado.");
        }
    }
}