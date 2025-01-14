using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace keycontrol.Infrastructure.Repositories;

public class RoomRepository : IRoomRepository
{
    private readonly AppDbContext _context;

    public RoomRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Room> GetRoomByExternalId(Guid externalId)
    {
        return await _context.rooms.Where(r => r.ExternalId.Equals(externalId)).FirstOrDefaultAsync();
    }
}