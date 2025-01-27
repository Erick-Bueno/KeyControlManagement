using keycontrol.Application.Authentication.Common.Interfaces.Authentication;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Errors.DomainErrors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using MediatR;
using OneOf;

namespace keycontrol.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, OneOf<LoginResponse, AppError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IBcrypt _bcrypt;
    private readonly ITokenJwtGenerator _tokenJwtGenerator;
    private readonly ITokenRepository _tokenRepository;

    public LoginQueryHandler(IUserRepository userRepository, IBcrypt bcrypt, ITokenJwtGenerator tokenJwtGenerator, ITokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _bcrypt = bcrypt;
        _tokenJwtGenerator = tokenJwtGenerator;
        _tokenRepository = tokenRepository;
    }

    public async Task<OneOf<LoginResponse, AppError>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            return new InvalidEmail(emailResult.ErrorMessage);
        }
        var user = await _userRepository.FindUserByEmail(emailResult.Value);
        if (user is null)
        {
            return new UserNotRegistered("User not registered");
        }
        var passwordResult = Password.Create(request.Password);
        if (passwordResult.IsFailure)
        {
            return new InvalidPassword(passwordResult.ErrorMessage);
        }

        var passwordIsValid = _bcrypt.VerifyPassword(request.Password, user.Password);
        if (!passwordIsValid)
        {
            return new InvalidPassword("Invalid password");
        }
        var accessToken = _tokenJwtGenerator.GenerateAccessToken(user.ExternalId);
        var refreshToken = _tokenJwtGenerator.GenerateRefreshToken();

        await ManageLoginToken(user, refreshToken);

        return new LoginResponse(user.ExternalId, user.Name, user.Email.EmailValue, accessToken, refreshToken);
    }
    private async Task ManageLoginToken(User user, string refreshToken)
    {

        var findedToken = await _tokenRepository.FindTokenByEmail(user.Email);

        if (findedToken is not null)
        {
            await _tokenRepository.UpdateToken(findedToken, refreshToken);
        }
        else
        {
            var newToken = Token.Create(user.Email, refreshToken);
            await _tokenRepository.AddToken(newToken);
        }
    }
}
