using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using MediatR;
using OneOf;

namespace keycontrol.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OneOf<RegisterResponse, AppError>>
{
    public Task<OneOf<RegisterResponse, AppError>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userFinded = await _userRepository.FindUserByEmail(request.Email);

        if (userFinded != null)
        {
            return new UserAlreadyRegistered("User already registered");
        }
        var encryptPassword = _bcryptNet.EncryptPassword(request.Password);

        var newUser = User.CreateUser(request.Email, encryptPassword, request.UserName);

        var user = await  _userRepository.AddUser(newUser);

        return new RegisterResponse(user.Id, user.Username, user.Email);
    }
}