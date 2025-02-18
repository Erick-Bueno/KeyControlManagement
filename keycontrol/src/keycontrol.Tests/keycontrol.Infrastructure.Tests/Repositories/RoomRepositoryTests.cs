using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Repositories;
using keycontrol.Tests.Extensions;
using keycontrol.Tests.Helpers;
using System.Threading.Tasks;
using Xunit;

namespace keycontrol.Tests.Repositories;
[Trait("Category", "RoomRepository")]
public class RoomRepositoryTests : DatabaseUnitTest
{
    private readonly RoomRepository _roomRepository;
    private readonly Faker _faker = new Faker("pt_BR");
    public RoomRepositoryTests()
    {
        _roomRepository = new RoomRepository(_dbContext);
    }
    [Fact]
    public async Task AddRoom_GivenValidRoom_ThenCreateRoomAsync()
    {
        var room = Room.Create(_faker.Lorem.Text());

        await _roomRepository.AddRoom(room.Value);

        var foundRoom = await _dbContext.rooms.FindAsync(room.Value.Id);

        foundRoom.Should().NotBeNull();
        foundRoom.Should().Be(room.Value);
    }
    [Fact]
    public async Task GetRoomByExternalId_GivenExternalId_ThenReturnRoomAsync()
    {
        var room = Room.Create(_faker.Lorem.Text());
        await _dbContext.rooms.AddAsync(room.Value);
        await _dbContext.SaveChangesAsync();

        var foundRoom = await _roomRepository.GetRoomByExternalId(room.Value.ExternalId);

        foundRoom.Should().NotBe(null);
        foundRoom.Should().Be(room.Value);
    }
}