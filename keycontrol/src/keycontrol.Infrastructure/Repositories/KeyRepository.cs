using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


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

    public async Task<KeyRoom> FindKeyByExternalId(Guid externalId)
    {
        return await _context.keys.Where(k => k.ExternalId == externalId).FirstOrDefaultAsync();
    }

    public async Task<KeyRoom> FindUserById(int id)
    {
        return await _context.keys.FindAsync(id);
    }
}