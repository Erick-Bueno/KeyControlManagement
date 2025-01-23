using Bogus;
using FluentAssertions;
using keycontrol.Application.Errors;
using keycontrol.Application.Key.Commands;
using keycontrol.Application.Key.Responses;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using Moq;
using Xunit;

namespace keycontrol.Tests.Application.Key;

public class RegisterKeyCommandHandlerTests
{
    private readonly Mock<IRoomRepository> _roomRepositoryMock;
    private readonly Mock<IKeyRepository> _keyRepositoryMock;
    private readonly RegisterKeyCommandHandler _registerKeyCommandHandler;
    private readonly Faker _faker = new Faker("pt_BR");
    public RegisterKeyCommandHandlerTests()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _keyRepositoryMock = new Mock<IKeyRepository>();
        _registerKeyCommandHandler = new RegisterKeyCommandHandler(_roomRepositoryMock.Object, _keyRepositoryMock.Object);
    }

    [Fact]
    [Trait("Category", "KeyCommandHandler")]
    public async Task Handle_GivenRoomDoesNotExist_ThenReturnRoomNotFindedError()
    {
        var externalIdRoomNotRegistered = _faker.Random.Guid();
        var registerKeyCommand = new RegisterKeyCommand(externalIdRoomNotRegistered, _faker.Lorem.Text()); 
        _roomRepositoryMock.Setup(r => r.GetRoomByExternalId(externalIdRoomNotRegistered)).ReturnsAsync((Room)null);        
        
        var result = await _registerKeyCommandHandler.Handle(registerKeyCommand, CancellationToken.None);
        var expectedResponseError = new RoomNotFinded("Room not found");
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
    }
    [Fact]
    [Trait("Category", "KeyCommandHandler")]
    public async Task Handle_GivenRoomDoesExist_ThenAddNewKeyRoom()
    {
        var registerKeyCommand = new RegisterKeyCommand(_faker.Random.Guid(), _faker.Lorem.Text());
        var room = Room.Create(_faker.Lorem.Text());
        var key = KeyRoom.Create(room.Value.Id,registerKeyCommand.Description);
        _roomRepositoryMock.Setup(r => r.GetRoomByExternalId(registerKeyCommand.ExternalIdRoom)).ReturnsAsync(room.Value);
        _keyRepositoryMock.Setup(k => k.AddKey(key.Value));
        
        var result = await _registerKeyCommandHandler.Handle(registerKeyCommand, CancellationToken.None);
        var expectedResponse = new RegisterKeyResponse(key.Value.ExternalId, room.Value.ExternalId, key.Value.Description, room.Value.Name);
        
        result.AsT0.Description.Should().Be(expectedResponse.Description);
        result.AsT0.Room.Should().Be(expectedResponse.Room);
        result.AsT0.ExternalIdRoom.Should().Be(expectedResponse.ExternalIdRoom);
    }
}