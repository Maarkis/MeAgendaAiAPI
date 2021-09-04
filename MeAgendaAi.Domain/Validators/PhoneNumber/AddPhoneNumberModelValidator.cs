using FluentValidation;
using MeAgendaAi.Domain.EpModels.PhoneNumber;

namespace MeAgendaAi.Domain.Validators.PhoneNumber
{
    public class AddPhoneNumberModelValidator : AbstractValidator<AddPhoneNumberModel>
    {
        public AddPhoneNumberModelValidator()
        {
            RuleFor(x => x.Number)
                .NotNull()
                .NotEmpty()
                .WithMessage("Número de telefone é obrigátorio");
        }
    }
}