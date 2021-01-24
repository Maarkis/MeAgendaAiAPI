using FluentValidation;
using MeAgendaAi.Domain.EpModels.PhoneNumber;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Validators.PhoneNumber
{
    public class AddPhoneNumberModelValidator : AbstractValidator<AddPhoneNumberModel>
    {
        public AddPhoneNumberModelValidator()
        {
            RuleFor(x => x.Number)
                .NotNull()
                .NotEmpty()
                .WithMessage("");
        }
    }
}
