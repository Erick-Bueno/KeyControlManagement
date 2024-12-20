using FluentValidation;

namespace keycontrol.Application.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid Email");
        RuleFor(l => l.Password)
            .NotEmpty()
            .Matches(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-_@#$%]).{8,}").WithMessage("The password must contain at least 8 characters, a stored letter, a number and a special character");
        RuleFor(r => r.Username)
            .NotEmpty();
    }
}