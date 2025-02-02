using Bogus;
using FluentAssertions;
using keycontrol.Application.Authentication.Commands.Register;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
<<<<<<< HEAD
=======
using keycontrol.Domain.ValueObjects;
using keycontrol.Tests.Fakers;
>>>>>>> 95c19ed2d672d594eda56d57f76f2d6be4472b5f
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Xunit;

namespace keycontrol.Tests.Authentication.Commands.Register;
<<<<<<< HEAD

=======
[Trait("Category", "RegisterCommandHandler")]
>>>>>>> 95c19ed2d672d594eda56d57f76f2d6be4472b5f
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
<<<<<<< HEAD
    [Trait("Category", "RegisterCommandHandler")]
    public async Task Handle_GivenUserAlreadyExists_ThenReturnUserAlreadyRegisteredError()
    {
        var userAlreadyRegistered =
            new User(_faker.Person.FirstName, _faker.Person.Email, _faker.Random.AlphaNumeric(8));
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(userAlreadyRegistered.Email))
            .ReturnsAsync(userAlreadyRegistered);
        var request =
            new RegisterCommand(userAlreadyRegistered.Name, userAlreadyRegistered.Email,
                userAlreadyRegistered.Password);
=======
    public async Task Handle_GivenUserAlreadyExists_ThenReturnUserAlreadyRegisteredError()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var userAlreadyRegistered =
            User.Create(_faker.Person.FirstName, emailValueObject.Value, _faker.Random.AlphaNumeric(8));
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(userAlreadyRegistered.Value.Email))
            .ReturnsAsync(userAlreadyRegistered.Value);
        var request =
            new RegisterCommand(userAlreadyRegistered.Value.Name, userAlreadyRegistered.Value.Email.EmailValue,
                userAlreadyRegistered.Value.Password);
>>>>>>> 95c19ed2d672d594eda56d57f76f2d6be4472b5f
        var expectedResponseError = new UserAlreadyRegistered("User already registered");

        var result = await _registerCommandHandler.Handle(request, CancellationToken.None);

        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
    }

    [Fact]
<<<<<<< HEAD
    [Trait("Category", "RegisterCommandHandler")]
    public async Task Handle_GivenUserDoesNotExist_ThenRegisterUser()
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
        
=======
    public async Task Handle_GivenUserDoesNotExist_ThenRegisterUser()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var userNotRegistered =
        User.Create(_faker.Person.FirstName, emailValueObject.Value, ValidPassword.Generate());
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(userNotRegistered.Value.Email))
            .ReturnsAsync((User)null);
        _userRepositoryMock.Setup(ur => ur.AddUser(It.IsAny<User>())).ReturnsAsync(userNotRegistered.Value);
        var request =
            new RegisterCommand(userNotRegistered.Value.Name, userNotRegistered.Value.Email.EmailValue,
                userNotRegistered.Value.Password);
        var expectedResponseSuccess = new RegisterResponse(userNotRegistered.Value.ExternalId, userNotRegistered.Value.Name, userNotRegistered.Value.Email.EmailValue);

        var result = await _registerCommandHandler.Handle(request, CancellationToken.None);

>>>>>>> 95c19ed2d672d594eda56d57f76f2d6be4472b5f
        result.AsT0.Email.Should().Be(expectedResponseSuccess.Email);
        result.AsT0.ExternalId.Should().Be(expectedResponseSuccess.ExternalId);
        result.AsT0.Name.Should().Be(expectedResponseSuccess.Name);
    }
<<<<<<< HEAD
=======
    [Fact]
    public async Task Handle_GivenInvalidEmailToDomain_ReturnErrorInvalidEmailAsync()
    {
        var invalidEmail = _faker.Random.AlphaNumeric(8);
        var registerCommand = new RegisterCommand(_faker.Person.UserName, invalidEmail, ValidPassword.Generate());
        var result = await _registerCommandHandler.Handle(registerCommand, CancellationToken.None);

        result.AsT1.NameError.Should().Be("InvalidEmail");
        result.AsT1.ErrorType.Should().Be("BadRequest");
    }
    [Fact]
    public async Task Handle_GivenInvalidPasswordToDomain_ReturnErrorInvalidPasswordAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var invalidPassword = _faker.Random.AlphaNumeric(8);
        var registerCommand = new RegisterCommand(_faker.Person.UserName, email, invalidPassword);
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(emailValueObject.Value)).ReturnsAsync((User)null);

        var result = await _registerCommandHandler.Handle(registerCommand, CancellationToken.None);

        result.AsT1.NameError.Should().Be("InvalidPassword");
        result.AsT1.ErrorType.Should().Be("BadRequest");
    }
    [Fact]
    public async Task Handle_GivenInvalidNameToDomain_ReturnErrorFailCreateUserAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var userNotRegistered =
        User.Create(_faker.Person.FirstName, emailValueObject.Value, ValidPassword.Generate());
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(userNotRegistered.Value.Email))
            .ReturnsAsync((User)null);
        _userRepositoryMock.Setup(ur => ur.AddUser(It.IsAny<User>())).ReturnsAsync(userNotRegistered.Value);
        var request =
            new RegisterCommand(" ", userNotRegistered.Value.Email.EmailValue,
                userNotRegistered.Value.Password);

        var result = await _registerCommandHandler.Handle(request, CancellationToken.None);

        result.AsT1.NameError.Should().Be("FailCreateUser");
        result.AsT1.ErrorType.Should().Be("BadRequest");
       
    }
>>>>>>> 95c19ed2d672d594eda56d57f76f2d6be4472b5f
}