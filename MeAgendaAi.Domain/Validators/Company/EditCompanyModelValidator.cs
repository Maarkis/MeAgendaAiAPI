using FluentValidation;
using MeAgendaAi.Domain.EpModels.Company;
using MeAgendaAi.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Validators.Company
{
    public class EditCompanyModelValidator : AbstractValidator<EditCompanyModel>
    {
        public EditCompanyModelValidator()
        {
            RuleFor(x => x.UserId)
             .NotNull()
             .NotEmpty()
             .Must(id => GuidUtil.IsGuidValid(id))
             .WithMessage("UserId inválido");

            RuleFor(x => x.LimitCancelHours)
                .NotNull()
                .WithMessage("Campo LimitCancelhours não pode ser nulo");
        }
    }
}
