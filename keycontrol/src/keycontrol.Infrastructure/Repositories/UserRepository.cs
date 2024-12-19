using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;

namespace keycontrol.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<User> FindUserByEmail(string email)
    {
        throw new NotImplementedException();
    }

    public Task<User> AddUser(User user)
    {
        throw new NotImplementedException();
    }
}