namespace keycontrol.Domain.Entities;

public class Token : Entity
{
    public Token(string email, string refreshToken)
    {
        Email = email;
        RefreshToken = refreshToken;
    }

    public string Email { get; private set; }
    public string RefreshToken { get; private set; }

    public static Token UpdateToken(Token token, string refreshToken){
        token.RefreshToken = refreshToken;
        return token;
    }
}