using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Repositories;
using keycontrol.Tests.Helpers;
using Xunit;
namespace keycontrol.Tests.Repositories;
[Trait("Category", "KeyRepository")]
public class KeyRepositoryTests : DatabaseUnitTest
{
    private readonly Faker _faker = new Faker("pt_BR");
    private readonly KeyRepository _keyRepository;

    public KeyRepositoryTests()
    {
        _keyRepository = new KeyRepository(_dbContext);
    }

    [Fact]
    public async Task AddKey_GivenValidKey_ThenCreateKeyAsync()
    {
        var key = KeyRoom.Create(_faker.Random.Number(1, 5), _faker.Lorem.Text());

        await _keyRepository.AddKey(key.Value);
        var keyFound = await _dbContext.keys.FindAsync(key.Value.Id);

        keyFound.Should().NotBeNull();
        keyFound.Should().Be(key.Value);

    }
    [Fact]
    public async Task FindKeyByExternalId_GivenExternalId_ThenReturnKeyAsync()
    {
        var key = KeyRoom.Create(_faker.Random.Number(1, 5), _faker.Lorem.Text());

        await _dbContext.keys.AddAsync(key.Value);
        await _dbContext.SaveChangesAsync();

        var result = await _keyRepository.FindKeyByExternalId(key.Value.ExternalId);

        result.Should().NotBeNull();
        result.Should().Be(key.Value);
    }
}