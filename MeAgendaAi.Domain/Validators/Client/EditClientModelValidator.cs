using FluentValidation;
using MeAgendaAi.Domain.EpModels.Client;
using MeAgendaAi.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MeAgendaAi.Domain.Validators.Client
{
    public class EditClientModelValidator : AbstractValidator<EditClientModel>
    {
        public EditClientModelValidator()
        {
            RuleFor(x => x.UserId)
                .NotNull()
                .NotEmpty()
                .Must(id => GuidUtil.IsGuidValid(id))
                .WithMessage("UserId inválido");

            RuleFor(x => x.RG)
                .NotNull()
                .NotEmpty()
                .Must(rg => {
                    Regex rx = new Regex(@"(^\d{1,2}).?(\d{3}).?(\d{3})-?(\d{1}|X|x$)", RegexOptions.None);
                    return rx.IsMatch(rg);
                })
                .WithMessage("RG com formato inválido");
                
        }
    }
}
