using keycontrol.Domain.Entities;

namespace keycontrol.Application.Repositories;

public interface IRoomRepository
{
    public Task<Room> GetRoomByExternalId(Guid externalId);
}