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
    [Fact]
    public void Create_GivenUserValidData_ThenShouldReturnUser()
    {
        var name = _faker.Person.FullName;
        var email = Email.Create(_faker.Person.Email).Value;
        var password = _faker.Random.AlphaNumeric(8);

        var result = User.Create(name, email, password);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(name);
        result.Value.Email.Should().Be(email);
        result.Value.Password.Should().Be(password);
    }
}
