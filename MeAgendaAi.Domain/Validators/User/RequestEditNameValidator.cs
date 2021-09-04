using FluentValidation;
using MeAgendaAi.Domain.EpModels.User;

namespace MeAgendaAi.Domain.Validators.User
{
    public class RequestEditNameValidator : AbstractValidator<RequestEditName>
    {
        public RequestEditNameValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("O campo Id não pode ser nulo.");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("O campo Nome não pode ser nulo.")
                .NotEmpty().WithMessage("O campo Nome não pode ser vazio");
        }
    }
}