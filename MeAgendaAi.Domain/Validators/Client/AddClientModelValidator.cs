using System.Text.RegularExpressions;
using FluentValidation;
using MeAgendaAi.Domain.EpModels.Client;

namespace MeAgendaAi.Domain.Validators.Client
{
    public class AddClientModelValidator : AbstractValidator<AddClientModel>
    {
        public AddClientModelValidator()
        {
            RuleFor(x => x.RG)
                .NotNull()
                .NotEmpty()
                .Must(rg =>
                {
                    var rx = new Regex(@"(^\d{1,2}).?(\d{3}).?(\d{3})-?(\d{1}|X|x$)", RegexOptions.None);
                    return rx.IsMatch(rg);
                })
                .WithMessage("RG com formato inválido");

            RuleFor(x => x.CPF)
                .NotNull()
                .NotEmpty()
                .Must(cpf =>
                {
                    var rx = new Regex(@"[0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2}", RegexOptions.None);
                    return rx.IsMatch(cpf);
                })
                .WithMessage("CPF com formato inválido");
        }
    }
}