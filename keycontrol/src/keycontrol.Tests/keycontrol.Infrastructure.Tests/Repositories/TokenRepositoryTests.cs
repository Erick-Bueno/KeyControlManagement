using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using keycontrol.Infrastructure.Repositories;
using keycontrol.Tests.Helpers;
using Xunit;

namespace keycontrol.Tests.Repositories;
[Trait("Category", "TokenRepository")]
public class TokenRepositoryTests : DatabaseUnitTest
{
    private readonly TokenRepository _tokenRepository;
    private readonly Faker _faker = new Faker("pt_BR");

    public TokenRepositoryTests()
    {

        _tokenRepository = new TokenRepository(_dbContext);
    }
    [Fact]
    public async Task FindTokenByEmail__GivenNotRegisteredEmail_ThenReturnNullAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var token = Token.Create(emailValueObject.Value, _faker.Random.AlphaNumeric(8));
        var unregisteredEmail = _faker.Person.Email + "unregistered";
        var unregisteredEmailValueObject = Email.Create(unregisteredEmail);
        _dbContext.tokens.Add(token);
        await _dbContext.SaveChangesAsync();

        var result = await _tokenRepository.FindTokenByEmail(unregisteredEmailValueObject.Value);

        result.Should().BeNull();
    }
    [Fact]
    public async Task FindTokenByEmail_GivenRegisteredEmail_ThenReturnTokenAsync()
    {
        var registeredEmail = _faker.Person.Email;
        var emailValueObject = Email.Create(registeredEmail);
        var token = Token.Create(emailValueObject.Value, _faker.Random.AlphaNumeric(8));
        _dbContext.tokens.Add(token);
        await _dbContext.SaveChangesAsync();

        var result = await _tokenRepository.FindTokenByEmail(emailValueObject.Value);

        result.Email.Should().Be(token.Email);
        result.ExternalId.Should().Be(token.ExternalId);
        result.Id.Should().Be(token.Id);
        result.RefreshToken.Should().Be(token.RefreshToken);
    }
    [Fact]
    public async Task AddToken_GivenToken_ThenAddTokenAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);

        var token = Token.Create(emailValueObject.Value, _faker.Random.AlphaNumeric(8));

        await _tokenRepository.AddToken(token);

        var tokenFinded = await _dbContext.tokens.FindAsync(token.Id);
        tokenFinded.Should().NotBeNull();
        tokenFinded.Email.Should().Be(token.Email);
        tokenFinded.ExternalId.Should().Be(token.ExternalId);
        tokenFinded.Id.Should().Be(token.Id);
        tokenFinded.RefreshToken.Should().Be(token.RefreshToken);
    }
    [Fact]
    public async Task UpdateToken_GivenTokenAndRefreshToken_ThenUpdateTokenAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);

        var token = Token.Create(emailValueObject.Value, _faker.Random.AlphaNumeric(8));
        var newRefreshToken = _faker.Random.AlphaNumeric(8);
        _dbContext.tokens.Add(token);
        await _dbContext.SaveChangesAsync();

        await _tokenRepository.UpdateToken(token, newRefreshToken);

        var tokenFinded = await _dbContext.tokens.FindAsync(token.Id);

        tokenFinded.RefreshToken.Should().Be(newRefreshToken);
    }
}