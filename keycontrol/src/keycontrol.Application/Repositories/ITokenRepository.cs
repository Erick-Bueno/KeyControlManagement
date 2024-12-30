using keycontrol.Domain.Entities;

namespace keycontrol.Application.Repositories;

public interface ITokenRepository{
    Task<Token> FindTokenByEmail(string email);
    Task AddToken(Token token);
    Task UpdateToken(Token token, string newRefreshToken);
}