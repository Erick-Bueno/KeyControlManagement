﻿using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace keycontrol.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<User> FindUserByEmail(string email)
    {
        return await _appDbContext.users.Where(u => u.Email.Equals(email)).FirstOrDefaultAsync();
    }

    public async Task<User> AddUser(User user)
    {
        await _appDbContext.users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
        return user;
    }
}