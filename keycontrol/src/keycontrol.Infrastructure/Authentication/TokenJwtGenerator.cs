using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using keycontrol.Application.Authentication.Common.Interfaces.Authentication;
using keycontrol.Application.Authentication.Common.Interfaces.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace keycontrol.Infrastructure.Authentication;

public class TokenJwtGenerator : ITokenJwtGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly JwtSettings _jwtSettings;

    public TokenJwtGenerator(IDateTimeProvider dateTimeProvider, IOptions<JwtSettings> options)
    {
        _dateTimeProvider = dateTimeProvider;
        _jwtSettings = options.Value;
    }

    public string GenerateAccessToken(Guid userId)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
        };
        var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secretkey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddHours(_jwtSettings.ExpiresInAccessToken),
            claims: claims,
            signingCredentials: signinCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secretkey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            expires: _dateTimeProvider.UtcNow.AddHours(_jwtSettings.ExpiresInRefreshToken),
            signingCredentials: signinCredentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}