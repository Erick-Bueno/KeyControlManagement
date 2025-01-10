using FluentValidation;
using keycontrol.Application.Authentication.Queries.Login;

namespace keycontrol.Application.Authentication.Queries;

public class LoginQueryValidator : AbstractValidator<LoginQuery>
{
    public LoginQueryValidator()
    {
        RuleFor(l => l.Email)
         .NotEmpty()
         .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("Invalid Email");
        RuleFor(l => l.Password)
           .NotEmpty()
           .Matches(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[-_@#$%]).{8,}").WithMessage("The password must contain at least 8 characters, a stored letter, a number and a special character");
    }
}