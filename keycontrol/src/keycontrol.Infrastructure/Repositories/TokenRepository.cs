using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using keycontrol.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace keycontrol.Infrastructure.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _context;

        public TokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Token> FindTokenByEmail(Email email)
        {
            return await _context.tokens.Where(t => t.Email == email).FirstOrDefaultAsync();
        }

        public async Task AddToken(Token token)
        {
            await _context.tokens.AddAsync(token);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateToken(Token token, string newRefreshToken)
        {
            Token.Update(token, newRefreshToken);
            await _context.SaveChangesAsync();
        }
    }
}