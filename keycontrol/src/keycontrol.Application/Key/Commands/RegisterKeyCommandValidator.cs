using FluentValidation;

namespace keycontrol.Application.Key.Commands;

public class RegisterKeyCommandValidator : AbstractValidator<RegisterKeyCommand>
{
    public RegisterKeyCommandValidator()
    {
        RuleFor(x => x.ExternalIdRoom)
            .NotEmpty().WithMessage("ExternalIdRoom is required.");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");
    }
}
