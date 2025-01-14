using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Context;

namespace keycontrol.Infrastructure.Repositories;

public class KeyRepository : IKeyRepository
{
    private readonly AppDbContext _context;

    public KeyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddKey(KeyRoom key)
    {
        await _context.keys.AddAsync(key);
        await _context.SaveChangesAsync();
    }
}