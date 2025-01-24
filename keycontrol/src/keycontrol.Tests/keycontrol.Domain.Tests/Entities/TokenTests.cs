using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using Xunit;

namespace keycontrol.Tests.Keycontrol.Domain.Tests.Entities;
[Trait("Category", "Entities")]
public class TokenTests
{
    private readonly Faker _faker = new Faker("pt_BR");

    [Fact]
    public void Create_GivenTokensData_ThenShouldCreateToken()
    {
        var email = Email.Create(_faker.Internet.Email());
        var refreshToken = _faker.Random.String2(10);

        var result = Token.Create(email.Value, refreshToken);

        result.Email.EmailValue.Should().Be(email.Value.EmailValue);
        result.RefreshToken.Should().Be(refreshToken);
    }
    [Fact]
    public void Update_GivenTokensData_ThenShouldUpdateToken()
    {
        var token = Token.Create(Email.Create(_faker.Internet.Email()).Value, _faker.Random.String2(10));
        var refreshToken = _faker.Random.String2(10);

        var result = Token.Update(token, refreshToken);

        result.RefreshToken.Should().Be(refreshToken);
        result.Email.EmailValue.Should().Be(token.Email.EmailValue);
    }
}