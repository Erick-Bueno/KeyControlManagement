using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;

namespace keycontrol.Application.Repositories;

public interface IUserRepository
{
    public Task<User> FindUserByEmail(Email email);
    public Task<User> AddUser(User user);
}