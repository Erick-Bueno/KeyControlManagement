using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Domain.Enums;
using Xunit;

namespace keycontrol.Tests.Entities;
[Trait("Category", "Entities")]
public class KeyRoomTests
{
    private readonly Faker _faker = new Faker("pt_BR");
    [Fact]
    public void Create_GivenDescriptionNullOrEmpty_ThenReturnError ()
    {
        var idRoom = _faker.Random.Int();
        var description = string.Empty;
        var result = KeyRoom.Create(idRoom, description);
        result.ErrorMessage.Should().Be("Inform an description");
        result.IsFailure.Should().BeTrue();
    }
    [Fact]
    public void Create_GivenDescriptionNullOrWhiteSpace_ThenReturnError ()
    {
        var idRoom = _faker.Random.Int();
        var description = " ";
        var result = KeyRoom.Create(idRoom, description);
        result.ErrorMessage.Should().Be("Inform an description");
        result.IsFailure.Should().BeTrue();
    }
    [Fact]
    public void Create_GivenValidKeyRoomsData_ThenCreateKeyRoom ()
    {
        var idRoom = _faker.Random.Int();
        var description = _faker.Random.String2(10);
        var result = KeyRoom.Create(idRoom, description);
        result.IsSuccess.Should().BeTrue();
        result.Value.IdRoom.Should().Be(idRoom);
        result.Value.Description.Should().Be(description);
        result.Value.Status.Should().Be(Status.Available);
    }
}