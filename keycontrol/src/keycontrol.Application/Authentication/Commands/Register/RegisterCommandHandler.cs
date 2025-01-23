using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Errors.DomainErrors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
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
        var emailResult = Email.Create(request.Email);
        //testar
        if(emailResult.IsFailure){
            return new InvalidEmail(emailResult.ErrorMessage);
        }
        var isUserRegistered = await _userRepository.FindUserByEmail(emailResult.Value);

        if (isUserRegistered is not null)
        {
            return new UserAlreadyRegistered("User already registered");
        }
        var passwordResult = Password.Create(request.Password);
    //testar
        if(passwordResult.IsFailure){
            return new InvalidPassword(passwordResult.ErrorMessage);
        }

        var encryptPassword = _bcrypt.EncryptPassword(passwordResult.Value.PasswordValue);
        
        var newUser = User.Create(request.Username, emailResult.Value, encryptPassword);
        //testar
        if(newUser.IsFailure){
            return new FailCreateUser(newUser.ErrorMessage);
        }

        var user = await  _userRepository.AddUser(newUser.Value);

        return new RegisterResponse(user.ExternalId, user.Name, user.Email.EmailValue);
    }
}