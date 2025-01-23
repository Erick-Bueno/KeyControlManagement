using keycontrol.Domain.ValueObjects;

namespace keycontrol.Domain.Entities;

public class Token : Entity
{
    public Email Email { get; private set; }
    public string RefreshToken { get; private set; }
    private Token(Email email, string refreshToken)
    {
        Email = email;
        RefreshToken = refreshToken;
    }


    public static Token Update(Token token, string refreshToken)
    {
        token.RefreshToken = refreshToken;
        return token;
    }
    public static Token Create(Email email, string refreshToken)
    {
        return new Token(email, refreshToken);
    }
}