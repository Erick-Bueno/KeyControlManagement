using FluentValidation;

namespace keycontrol.Application.Keys.Commands.RegisterKey;

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
