using keycontrol.Domain.Entities;

namespace keycontrol.Application.Repositories;

public interface IKeyRepository
{
    public Task AddKey (KeyRoom key);
}