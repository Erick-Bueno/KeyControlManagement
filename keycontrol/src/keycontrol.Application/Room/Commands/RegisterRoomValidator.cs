using FluentValidation;

namespace keycontrol.Application.Room.Commands;

public class RegisterRoomValidator : AbstractValidator<RegisterRoomCommand>
{
    public RegisterRoomValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}