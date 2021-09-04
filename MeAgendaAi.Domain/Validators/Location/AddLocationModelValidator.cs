using FluentValidation;
using MeAgendaAi.Domain.EpModels.Location;

namespace MeAgendaAi.Domain.Validators.Location
{
    public class AddLocationModelValidator : AbstractValidator<AddLocationModel>
    {
        public AddLocationModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("O campo Nome não pode ser nulo");

            RuleFor(x => x.Country)
                .NotNull()
                .WithMessage("O campo País não pode ser nulo");

            RuleFor(x => x.State)
                .NotNull()
                .WithMessage("O campo Estado não pode ser nulo");

            RuleFor(x => x.City)
                .NotNull()
                .WithMessage("O campo Cidade não pode ser nulo");

            RuleFor(x => x.Neighbourhood)
                .NotNull()
                .WithMessage("O campo Bairro não pode ser nulo");

            RuleFor(x => x.Street)
                .NotNull()
                .WithMessage("O campo Rua não pode ser nulo");

            RuleFor(x => x.Number)
                .NotNull()
                .WithMessage("O campo Número não pode ser nulo");

            RuleFor(x => x.CEP)
                .NotNull()
                .WithMessage("O campo CEP não pode ser nulo");
        }
    }
}