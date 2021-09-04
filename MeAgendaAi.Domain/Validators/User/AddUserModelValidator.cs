using System.Text.RegularExpressions;
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
                .NotEmpty()
                .WithMessage("O campo Nome não pode ser nulo");

            RuleFor(x => x.Email)
                .NotNull()
                .NotEmpty()
                .WithMessage("O campo Email não pode ser nulo");

            RuleFor(x => x.Email)
                .NotNull()
                .Must(cpf =>
                {
                    var rx = new Regex(@"^\S+@\S+$", RegexOptions.None);
                    return rx.IsMatch(cpf);
                })
                .WithMessage("Email com formato inválido");

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage("O campo Senha não pode ser nulo");
        }
    }
}