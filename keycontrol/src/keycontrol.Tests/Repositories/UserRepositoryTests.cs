using Bogus;
using FluentAssertions;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Context;
using keycontrol.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace keycontrol.Tests.Repositories;

public class UserRepositoryTests
{
    private readonly AppDbContext _dbContext;
    private readonly UserRepository _userRepository;
    private readonly Faker _faker = new Faker("pt_BR");
    public UserRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite("Filename=UserRepositoryTests.db")
        .Options;   

        _dbContext = new AppDbContext(dbContextOptions);

        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();

        _userRepository = new UserRepository(_dbContext);
    }
    [Fact]
    [Trait("Category", "UserRepository")]
    public async Task FindUserByEmail_GivenNotRegisteredEmail_ThenReturnNullAsync()
    {
       var user = new User(_faker.Person.UserName, _faker.Person.Email, _faker.Random.AlphaNumeric(8));
       var unregisteredEmail = _faker.Person.Email + "unregistered";
       _dbContext.users.Add(user);
       await _dbContext.SaveChangesAsync();

       var result = await _userRepository.FindUserByEmail(unregisteredEmail);

       result.Should().BeNull();
    }
    [Fact]
    [Trait("Category", "UserRepository")]
    public async Task FindUserByEmail_GivenRegisteredEmail_ThenReturnUserAsync()
    {
       var registeredEmail = _faker.Person.Email;
       var user = new User(_faker.Person.UserName, registeredEmail, _faker.Random.AlphaNumeric(8));
       _dbContext.users.Add(user);
       await _dbContext.SaveChangesAsync();

       var result = await _userRepository.FindUserByEmail(registeredEmail);

       result.Email.Should().Be(user.Email);
       result.ExternalId.Should().Be(user.ExternalId);
       result.Id.Should().Be(user.Id);
       result.Name.Should().Be(user.Name);
       result.Password.Should().Be(user.Password);
    }
}