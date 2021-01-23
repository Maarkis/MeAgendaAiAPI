using FluentValidation;
using MeAgendaAi.Domain.EpModels.Employee;
using MeAgendaAi.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MeAgendaAi.Domain.Validators.Employee
{
    public class AddEmployeeModelValidator : AbstractValidator<AddEmployeeModel>
    {
        public AddEmployeeModelValidator() {
            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty()
                .WithMessage("O Campo CompanyId não pode ser nulo");

            RuleFor(x => x.CompanyId)
                .Must(companyId => {
                    return GuidUtil.IsGuidValid(companyId);
                })
                .WithMessage("CompanyId com formato inválido");

            RuleFor(x => x.IsManager)
                .NotNull()
                .WithMessage("O campo IsManager não pode ser nulo");

            RuleFor(x => x.RG)
                .NotNull()
                .NotEmpty()
                .WithMessage("O campo RG não pode ser nulo");

            RuleFor(x => x.RG)
                .NotNull()
                .Must(rg => {
                    Regex rx = new Regex(@"(^\d{1,2}).?(\d{3}).?(\d{3})-?(\d{1}|X|x$)", RegexOptions.None);
                    return rx.IsMatch(rg);
                })
                .WithMessage("CPF com formato inválido");

            RuleFor(x => x.CPF)
                .NotNull()
                .NotEmpty()
                .WithMessage("O campo CPF não pode ser nulo");

            RuleFor(x => x.CPF)
                .NotNull()
                .Must(cpf => {
                    Regex rx = new Regex(@"[0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2}", RegexOptions.None);
                    return rx.IsMatch(cpf);
                })
                .WithMessage("CPF com formato inválido");
        }
    }
}
