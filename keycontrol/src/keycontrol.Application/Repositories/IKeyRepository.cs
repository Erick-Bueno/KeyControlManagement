using keycontrol.Domain.Entities;

namespace keycontrol.Application.Repositories;

public interface IKeyRepository
{
    public Task AddKey (KeyRoom key);
    public Task<KeyRoom> FindKeyByExternalId(Guid externalId);
    public Task<KeyRoom> FindUserById(int id);
}