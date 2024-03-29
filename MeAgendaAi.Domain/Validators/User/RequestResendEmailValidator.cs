﻿using FluentValidation;
using MeAgendaAi.Domain.EpModels.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeAgendaAi.Domain.Validators.User
{
    public class RequestResendEmailValidator : AbstractValidator<RequestResendEmail>
    {
        public RequestResendEmailValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                    .WithMessage("E-mail é obrigatório")
                .EmailAddress()
                    .WithMessage("Email com formato inválido");
        }
    }
}
