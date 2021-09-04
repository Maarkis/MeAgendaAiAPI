using FluentValidation;
using MeAgendaAi.Domain.EpModels.User;

namespace MeAgendaAi.Domain.Validators.Authentication
{
    public class AuthenticationModelValidator : AbstractValidator<LoginModel>
    {
        public AuthenticationModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("E-mail obrigátorio")
                .EmailAddress()
                .WithMessage("Digite um e-mail válido");

            RuleFor(x => x.Senha)
                .NotEmpty()
                .WithMessage("A senha está vazia!");
        }
    }
}