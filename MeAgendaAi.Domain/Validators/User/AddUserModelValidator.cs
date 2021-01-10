using FluentValidation;
using MeAgendaAi.Domain.EpModels.User;

namespace MeAgendaAi.Domain.Validators.User
{
    public class AddUserModelValidator : AbstractValidator<AddUserModel>
    {
        public AddUserModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("O campo Nome não pode ser nulo");

            RuleFor(x => x.Email)
                .NotNull()
                .WithMessage("O campo Email não pode ser nulo");

            RuleFor(x => x.Password)
                .NotNull()
                .WithMessage("O campo Senha não pode ser nulo");

            RuleFor(x => x.RG)
                .NotNull()
                .WithMessage("O campo RG não pode ser nulo");

            RuleFor(x => x.CPF)
                .NotNull()
                .WithMessage("O campo CPF não pode ser nulo");
        }

    }
}
