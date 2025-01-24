using Bogus;
using FluentAssertions;
using keycontrol.Domain.ValueObjects;
using keycontrol.Tests.Fakers;
using Xunit;

namespace keycontrol.Tests.Keycontrol.Domain.Tests.ValueObjects;

public class PasswordTests
{
    private readonly Faker _faker = new Faker("pt_BR");

    [Fact]
    public void Create_GivenPasswordNullOrEmpty_ThenShouldReturnError()
    {
        var emptyPassword = string.Empty;

        var result = Password.Create(emptyPassword);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("inform an password");
    }
    [Fact]
    public void Create_GivenPasswordNullOrWhiteSpace_ThenShouldReturnError()
    {
        var emptyPassword = "";

        var result = Password.Create(emptyPassword);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("inform an password");
    }
    [Fact]
    public void Create_GivenInvalidPassword_ThenShouldReturnError()
    {
       var invalidPassword = _faker.Random.String2(7);

        var result = Password.Create(invalidPassword);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Invalid password");
    }
    [Fact]
    public void Create_GivenValidPassword_ThenShouldCreatePassword()
    {
        var validPassword = ValidPassword.Generate();

        var result = Password.Create(validPassword);

        result.IsSuccess.Should().BeTrue();
        result.Value.PasswordValue.Should().Be(validPassword);
    }
}