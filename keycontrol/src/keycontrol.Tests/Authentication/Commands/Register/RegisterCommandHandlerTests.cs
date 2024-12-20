using Bogus;
using FluentAssertions;
using keycontrol.Application.Authentication.Commands.Register;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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
        var userAlreadyRegistered =
            new User(_faker.Person.FirstName, _faker.Person.Email, _faker.Random.AlphaNumeric(8));
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(userAlreadyRegistered.Email))
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

    [Fact]
    [Trait("Category", "RegisterCommandHandler")]
    public async void Handle_GivenUserDoesNotExist_ThenRegisterUser()
    {
        var userNotRegistered =
            new User(_faker.Person.FirstName, _faker.Person.Email, _faker.Random.AlphaNumeric(8));
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(userNotRegistered.Email))
            .ReturnsAsync((User)null);
         _userRepositoryMock.Setup(ur => ur.AddUser(It.IsAny<User>())).ReturnsAsync(userNotRegistered);
        var request =
            new RegisterCommand(userNotRegistered.Name, userNotRegistered.Email,
                userNotRegistered.Password);
        var expectedResponseSuccess = new RegisterResponse(userNotRegistered.ExternalId,userNotRegistered.Name, userNotRegistered.Email);
        
        var result = await _registerCommandHandler.Handle(request, CancellationToken.None);
        
        result.AsT0.Email.Should().Be(expectedResponseSuccess.Email);
        result.AsT0.Id.Should().Be(expectedResponseSuccess.Id);
        result.AsT0.UserName.Should().Be(expectedResponseSuccess.UserName);
    }
}