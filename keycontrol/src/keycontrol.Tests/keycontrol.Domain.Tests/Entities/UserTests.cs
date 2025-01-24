using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using Xunit;

namespace keycontrol.Tests.Keycontrol.Domain.Tests.Entities;

public class UserTests
{
    private readonly Faker _faker = new Faker("pt_BR");
    [Fact]
    public void Create_GivenUserNameNullOrEmpty_ThenShouldReturnError()
    {
        var emptyName = string.Empty;
        var email = Email.Create(_faker.Person.Email).Value;
        var password = _faker.Random.AlphaNumeric(8);

        var result = User.Create(emptyName, email, password);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Inform an name");
    }
    [Fact]
    public void Create_GivenUserNameNullOrWhiteSpace_ThenShouldReturnError()
    {
        var emptyName = "";
        var email = Email.Create(_faker.Person.Email).Value;
        var password = _faker.Random.AlphaNumeric(8);

        var result = User.Create(emptyName, email, password);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Inform an name");
    }
}
