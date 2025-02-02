using Bogus;
using FluentAssertions;
using keycontrol.Application.Repositories;
using keycontrol.Application.Rooms.Commands.RegisterRoom;
using keycontrol.Application.Rooms.Responses;
using keycontrol.Domain.Entities;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace keycontrol.Tests.Application.Rooms;

public class RegisterRoomCommandHandlerTests
{

    private readonly Faker _faker = new Faker("pt_BR");
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly RegisterRoomCommandHandler _registerRoomCommandHandler;
    public RegisterRoomCommandHandlerTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _registerRoomCommandHandler = new RegisterRoomCommandHandler(_roomRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_GivenInvalidName_ThenReturnFailCreateKeyRoomErrorAsync()
    {
        var registerRoomCommand = new RegisterRoomCommand(" ");

        var result = await _registerRoomCommandHandler.Handle(registerRoomCommand, CancellationToken.None);

        result.AsT1.NameError.Should().Be("FailCreateKeyRoom");
        result.AsT1.ErrorType.Should().Be("BadRequest");

    }
    [Fact]
    public async Task Handle_GivenValidName_ThenCreateRoomAsync()
    {
        var registerRoomCommand = new RegisterRoomCommand(_faker.Random.String(10));
        var room = Room.Create(registerRoomCommand.Name);
        _roomRepositoryMock.Setup(x => x.AddRoom(room.Value)).ReturnsAsync(room.Value);
        var result = await _registerRoomCommandHandler.Handle(registerRoomCommand, CancellationToken.None);
        
        var expectedResponse = new RegisterRoomResponse(room.Value.ExternalId, room.Value.Name);
        result.AsT0.Name.Should().Be(expectedResponse.Name);
    } 
}