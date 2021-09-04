using FluentValidation;
using MeAgendaAi.Domain.EpModels.User;
using MeAgendaAi.Domain.Utils;

namespace MeAgendaAi.Domain.Validators.User
{
    public class EditUserModelValidator : AbstractValidator<EditUserModel>
    {
        public EditUserModelValidator()
        {
            RuleFor(x => x.UsuarioId)
                .NotNull()
                .NotEmpty()
                .Must(id => GuidUtil.IsGuidValid(id))
                .WithMessage("Guid inválido");

            RuleFor(x => x.Name)
                .NotNull()
                .WithMessage("O campo Nome não pode ser nulo");
        }
    }
}