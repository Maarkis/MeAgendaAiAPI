using FluentValidation;
using MeAgendaAi.Domain.EpModels.Company;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MeAgendaAi.Domain.Validators.Company
{
    public class AddCompanyModelValidator : AbstractValidator<AddCompanyModel>
    {
        public AddCompanyModelValidator()
        {
            RuleFor(x => x.CNPJ)
                .NotNull()
                .NotEmpty()
                .WithMessage("Campo CNPJ não pode ser nulo");

            RuleFor(x => x.CNPJ)
                .Must(cnpj => {
                    // apenas CNPJ => [0-9]{2}\.?[0-9]{3}\.?[0-9]{3}\/?[0-9]{4}\-?[0-9]{2}
                    // CPF e CNPJ => ([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})
                    // o campo recebe CPF e CNPJ

                    Regex rx = new Regex(@"([0-9]{2}[\.]?[0-9]{3}[\.]?[0-9]{3}[\/]?[0-9]{4}[-]?[0-9]{2})|([0-9]{3}[\.]?[0-9]{3}[\.]?[0-9]{3}[-]?[0-9]{2})", RegexOptions.None);
                    return rx.IsMatch(cnpj);
                })
                .WithMessage("CPNJ com formato inválido");

            RuleFor(x => x.LimitCancelHours)
                .NotNull()
                .WithMessage("Campo LimitCancelhours não pode ser nulo");
        }
    }
}
