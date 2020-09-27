using FluentValidation;
using MeAgendaAi.Domain.Entities;
using MeAgendaAi.Service.EpModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Service.Validators.User
{
    public class AddUserModelValidator : AbstractValidator<AddUserModel>
    {
        public AddUserModelValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("Nome não pode ser nulo");
        }

    }
}
