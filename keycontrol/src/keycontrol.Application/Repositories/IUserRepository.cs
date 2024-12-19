using keycontrol.Domain.Entities;

namespace keycontrol.Application.Repositories;

public interface IUserRepository
{
    public Task<User> FindUserByEmail(string email);
    public Task<User> AddUser(User user);
}