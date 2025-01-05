using Bogus;
using FluentAssertions;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Context;
using keycontrol.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace keycontrol.Tests.Repositories;

public class TokenRepositoryTests
{
    private readonly AppDbContext  _dbContext;
    private readonly ITokenRepository _tokenRepository;
    private readonly Faker _faker = new Faker("pt_BR");

    public TokenRepositoryTests()
    {
        var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
        .UseSqlite("Filename=TokenRepositoryTests.db")
        .Options;

        _dbContext = new AppDbContext(dbContextOptions);

        _dbContext.Database.OpenConnection();
        _dbContext.Database.EnsureCreated();

        _tokenRepository = new TokenRepository(_dbContext);
    }

    [Fact]
    [Trait("Category", "TokenRepository")]
    public async Task FindTokenByEmail__GivenNotRegisteredEmail_ThenReturnNullAsync()
    {
       var token = new Token(_faker.Person.Email, _faker.Random.AlphaNumeric(8));
       var unregisteredEmail = _faker.Person.Email + "unregistered";
       _dbContext.tokens.Add(token);
       await _dbContext.SaveChangesAsync();

       var result = await _tokenRepository.FindTokenByEmail(unregisteredEmail);

       result.Should().BeNull();
    }
}