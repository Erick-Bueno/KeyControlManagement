using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using MediatR;
using OneOf;

namespace keycontrol.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, OneOf<RegisterResponse, AppError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IBcrypt _bcrypt;

    public RegisterCommandHandler(IUserRepository userRepository, IBcrypt bcrypt)
    {
        _userRepository = userRepository;
        _bcrypt = bcrypt;
    }

    public async Task<OneOf<RegisterResponse, AppError>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var userFinded = await _userRepository.FindUserByEmail(request.Email);

        if (userFinded is not null)
        {
            return new UserAlreadyRegistered("User already registered");
        }
        var encryptPassword = _bcrypt.EncryptPassword(request.Password);
        
        var newUser = new User(request.Username, request.Email, encryptPassword);

        var user = await  _userRepository.AddUser(newUser);

        return new RegisterResponse(user.ExternalId, user.Name, user.Email);
    }
}