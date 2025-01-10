using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
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

        public async Task<Token> FindTokenByEmail(string email)
        {
            return await _context.tokens.Where(t => t.Email == email).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task AddToken(Token token)
        {
            await _context.tokens.AddAsync(token).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateToken(Token token, string newRefreshToken)
        {
            Token.UpdateToken(token, newRefreshToken);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}