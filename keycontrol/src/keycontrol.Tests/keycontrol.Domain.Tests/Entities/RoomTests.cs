using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using Xunit;

namespace keycontrol.Tests.Keycontrol.Domain.Tests.Entities;
[Trait("Category", "Entities")]
public class RoomTests
{
    private readonly Faker _faker = new Faker("pt_BR"); 

    [Fact]
    public void Create_GivenNameNullOrWhiteSpace_ThenReturnError()
    {
        var name = " ";

        var result = Room.Create(name);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Inform an name");
    }
    [Fact]
    public void Create_GivenNameNullOrEmpty_ThenReturnError()
    {
        var name = string.Empty;

        var result = Room.Create(name);

        result.IsFailure.Should().BeTrue();
        result.ErrorMessage.Should().Be("Inform an name");
    }
    [Fact]
    public void Create_GivenValidName_ThenCreateRoom()
    {
        var name = _faker.Random.String2(10);

        var result = Room.Create(name);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be(name);
    }
}