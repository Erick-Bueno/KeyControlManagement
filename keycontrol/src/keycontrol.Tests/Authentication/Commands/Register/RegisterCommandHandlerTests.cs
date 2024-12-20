using Bogus;
using FluentAssertions;
using keycontrol.Application.Authentication.Commands.Register;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Errors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using Moq;
using Xunit;

namespace keycontrol.Tests.Authentication.Commands.Register;

public class RegisterCommandHandlerTests
{
    private readonly RegisterCommandHandler _registerCommandHandler;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Faker _faker = new Faker("pt_BR");
    public RegisterCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        var bcryptMock = new Mock<IBcrypt>();
        _registerCommandHandler = new RegisterCommandHandler(_userRepositoryMock.Object, bcryptMock.Object);
    }
    [Fact]
    [Trait("Category", "RegisterCommandHandler")]
    public async void Handle_GivenUserAlreadyExists_ThenReturnUserAlreadyRegisteredError()
    {
        var userAlreadyRegistered = new User(_faker.Person.FirstName, _faker.Person.Email, _faker.Random.AlphaNumeric(8));
        var user = _userRepositoryMock.Setup(ur => ur.FindUserByEmail(userAlreadyRegistered.Email))
            .ReturnsAsync(userAlreadyRegistered);
        var request =
            new RegisterCommand(userAlreadyRegistered.Name, userAlreadyRegistered.Email,
                userAlreadyRegistered.Password);
        var expectedResponseError = new UserAlreadyRegistered("User already registered");

        var result = await _registerCommandHandler.Handle(request, CancellationToken.None);

        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
    }
}