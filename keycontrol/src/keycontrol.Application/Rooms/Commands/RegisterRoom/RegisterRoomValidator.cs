using FluentValidation;

namespace keycontrol.Application.Rooms.Commands.RegisterRoom;

public class RegisterRoomValidator : AbstractValidator<RegisterRoomCommand>
{
    public RegisterRoomValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}