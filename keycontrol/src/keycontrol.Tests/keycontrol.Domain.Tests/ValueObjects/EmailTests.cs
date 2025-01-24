using Bogus;
using FluentAssertions;
using keycontrol.Domain.ValueObjects;
using Xunit;

namespace keycontrol.Tests.Keycontrol.Domain.Tests.ValueObjects;

public class EmailTests
{
    private readonly Faker _faker = new Faker("pt_BR");

    [Fact]
    public void Create_GivenEmailNullOrEmpty_ThenShouldReturnError()
    {
        var emptyEmail = string.Empty;

        var result = Email.Create(emptyEmail);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("inform an email");
    }
    [Fact]
    public void Create_GivenEmailNullOrWhiteSpace_ThenShouldReturnError()
    {
       var emptyEmail = "";

        var result = Email.Create(emptyEmail);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("inform an email");
    }
    [Fact]
    public void Create_GivenInvalidEmail_ThenShouldReturnError()
    {
        var invalidEmail = _faker.Random.String2(10);
    
        var result = Email.Create(invalidEmail);
    
        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Invalid email");
    }
    [Fact]
    public void Create_GivenValidEmail_ThenCreateEmail()
    {
        var validEmail = _faker.Internet.Email();

        var result = Email.Create(validEmail);

        result.IsSuccess.Should().BeTrue();
        result.Value.EmailValue.Should().Be(validEmail);
    }
}