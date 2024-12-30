using keycontrol.Application.Authentication.Common.Interfaces.Authentication;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using MediatR;
using OneOf;

namespace keycontrol.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, OneOf<LoginResponse, AppError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IBcrypt _bcrypt;
    private readonly ITokenJwtGenerator tokenJwtGenerator;
    private readonly ITokenRepository _tokenRepository;

    public LoginQueryHandler(IUserRepository userRepository, IBcrypt bcrypt, ITokenJwtGenerator tokenJwtGenerator, ITokenRepository tokenRepository)
    {
        _userRepository = userRepository;
        _bcrypt = bcrypt;
        this.tokenJwtGenerator = tokenJwtGenerator;
        _tokenRepository = tokenRepository;
    }

    public async Task<OneOf<LoginResponse, AppError>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindUserByEmail(request.Email);
        if (user is null)
        {
            return new UserNotRegistered("User not registered");
        }
        var passwordIsValid = _bcrypt.VerifyPassword(request.Password, user.Password);

        if (!passwordIsValid)
        {
            return new InvalidPassword("Invalid password");
        }
        var accessToken = tokenJwtGenerator.GenerateAccessToken(user.ExternalId);
        var refreshToken = tokenJwtGenerator.GenerateRefreshToken();

        var findedToken = await _tokenRepository.FindTokenByEmail(user.Email);

        if (findedToken is not null)
        {
            await _tokenRepository.UpdateToken(findedToken, refreshToken);
        }
        else
        {
            var newToken = new Token(user.Email, refreshToken);
            await _tokenRepository.AddToken(newToken);
        }
        return new LoginResponse(user.ExternalId, user.Name, user.Email, accessToken, refreshToken);
    }
}
