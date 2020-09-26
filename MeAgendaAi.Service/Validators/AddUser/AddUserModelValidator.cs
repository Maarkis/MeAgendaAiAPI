using FluentValidation;
using MeAgendaAi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Validators.AddUser
{
    public class AddUserModelValidator : AbstractValidator<User>
    {
        public AddUserModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Nome não pode ser nulo");
        }

    }
}
