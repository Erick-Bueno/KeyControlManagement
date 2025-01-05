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
    private readonly AppDbContext _dbContext;
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
    [Fact]
    [Trait("Category", "TokenRepository")]
    public async Task FindTokenByEmail_GivenRegisteredEmail_ThenReturnTokenAsync()
    {
        var registeredEmail = _faker.Person.Email;
        var token = new Token(registeredEmail, _faker.Random.AlphaNumeric(8));
        _dbContext.tokens.Add(token);
        await _dbContext.SaveChangesAsync();

        var result = await _tokenRepository.FindTokenByEmail(registeredEmail);

        result.Email.Should().Be(token.Email);
        result.ExternalId.Should().Be(token.ExternalId);
        result.Id.Should().Be(token.Id);
        result.RefreshToken.Should().Be(token.RefreshToken);
    }
    [Fact]
    [Trait("Category", "TokenRepository")]
    public async Task AddToken_GivenToken_ThenAddTokenAsync()
    {
        var token = new Token(_faker.Person.Email, _faker.Random.AlphaNumeric(8));

        await _tokenRepository.AddToken(token);

        var tokenFinded = await _dbContext.tokens.FindAsync(token.Id);
        tokenFinded.Should().NotBeNull();
        tokenFinded.Email.Should().Be(token.Email);
        tokenFinded.ExternalId.Should().Be(token.ExternalId);
        tokenFinded.Id.Should().Be(token.Id);
        tokenFinded.RefreshToken.Should().Be(token.RefreshToken);
    }
}