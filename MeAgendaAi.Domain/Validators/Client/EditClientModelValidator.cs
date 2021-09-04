using System.Text.RegularExpressions;
using FluentValidation;
using MeAgendaAi.Domain.EpModels.Client;
using MeAgendaAi.Domain.Utils;

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
                .Must(rg =>
                {
                    var rx = new Regex(@"(^\d{1,2}).?(\d{3}).?(\d{3})-?(\d{1}|X|x$)", RegexOptions.None);
                    return rx.IsMatch(rg);
                })
                .WithMessage("RG com formato inválido");
        }
    }
}