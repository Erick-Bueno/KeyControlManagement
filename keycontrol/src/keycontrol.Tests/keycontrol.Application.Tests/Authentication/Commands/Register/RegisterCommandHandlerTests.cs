﻿using Bogus;
using FluentAssertions;
using keycontrol.Application.Authentication.Commands.Register;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using keycontrol.Tests.Fakers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Xunit;

namespace keycontrol.Tests.Authentication.Commands.Register;

[Trait("Category", "RegisterCommandHandler")]
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
        var expectedResponseError = new UserAlreadyRegistered("User already registered");

        var result = await _registerCommandHandler.Handle(request, CancellationToken.None);

        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
    }

    [Fact]
    [Trait("Category", "RegisterCommandHandler")]
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
        result.AsT0.Email.Should().Be(expectedResponseSuccess.Email);
        result.AsT0.ExternalId.Should().Be(expectedResponseSuccess.ExternalId);
        result.AsT0.Name.Should().Be(expectedResponseSuccess.Name);
    }
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
}