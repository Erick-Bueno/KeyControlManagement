using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;

namespace keycontrol.Application.Repositories;

public interface ITokenRepository{
    Task<Token> FindTokenByEmail(Email email);
    Task AddToken(Token token);
    Task UpdateToken(Token token, string newRefreshToken);
}