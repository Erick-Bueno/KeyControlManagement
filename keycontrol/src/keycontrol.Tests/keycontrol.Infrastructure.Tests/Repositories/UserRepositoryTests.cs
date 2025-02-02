using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using keycontrol.Infrastructure.Repositories;
using keycontrol.Tests.Helpers;
using Xunit;

namespace keycontrol.Tests.Repositories;
[Trait("Category", "UserRepository")]
public class UserRepositoryTests : DatabaseUnitTest
{
    private readonly UserRepository _userRepository;
    private readonly Faker _faker = new Faker("pt_BR");
    public UserRepositoryTests()
    {
        _userRepository = new UserRepository(_dbContext);
    }
    [Fact]
    public async Task FindUserByEmail_GivenNotRegisteredEmail_ThenReturnNullAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var user = User.Create(_faker.Person.UserName, emailValueObject.Value, _faker.Random.AlphaNumeric(8));
        var unregisteredEmail = _faker.Person.Email + "unregistered";
        var unregisteredEmailValueObject =  Email.Create(unregisteredEmail);
        _dbContext.users.Add(user.Value);
        await _dbContext.SaveChangesAsync();

        var result = await _userRepository.FindUserByEmail(unregisteredEmailValueObject.Value);

        result.Should().BeNull();
    }
    [Fact]
    public async Task FindUserByEmail_GivenRegisteredEmail_ThenReturnUserAsync()
    {
        var registeredEmail = _faker.Person.Email;
         var emailValueObject = Email.Create(registeredEmail);
        var user = User.Create(_faker.Person.UserName, emailValueObject.Value, _faker.Random.AlphaNumeric(8));
        _dbContext.users.Add(user.Value);
        await _dbContext.SaveChangesAsync();

        var result = await _userRepository.FindUserByEmail(emailValueObject.Value);

        result.Email.Should().Be(user.Value.Email);
        result.ExternalId.Should().Be(user.Value.ExternalId);
        result.Id.Should().Be(user.Value.Id);
        result.Name.Should().Be(user.Value.Name);
        result.Password.Should().Be(user.Value.Password);
    }
    [Fact]
    public async Task AddUser_GivenUser_ThenAddUserAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);

        var user = User.Create(_faker.Person.UserName, emailValueObject.Value, _faker.Random.AlphaNumeric(8));

        var result = await _userRepository.AddUser(user.Value);

        var userFinded = await _dbContext.users.FindAsync(user.Value.Id);
        result.Should().Be(userFinded);
    }
}