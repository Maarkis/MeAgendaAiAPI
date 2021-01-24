using FluentValidation;
using MeAgendaAi.Domain.EpModels.Employee;
using MeAgendaAi.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace MeAgendaAi.Domain.Validators.Employee
{
    public class EditEmployeeModelValidator : AbstractValidator<EditEmployeeModel>
    {
        public EditEmployeeModelValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotNull()
                .NotEmpty()
                .Must(id => GuidUtil.IsGuidValid(id))
                .WithMessage("UserId inválido");

            RuleFor(x => x.RG)
                .NotNull()
                .Must(rg => {
                    Regex rx = new Regex(@"(^\d{1,2}).?(\d{3}).?(\d{3})-?(\d{1}|X|x$)", RegexOptions.None);
                    return rx.IsMatch(rg);
                })
                .WithMessage("CPF com formato inválido");

            RuleFor(x => x.IsManager)
               .NotNull()
               .WithMessage("O campo IsManager não pode ser nulo");
        }
    }
}
