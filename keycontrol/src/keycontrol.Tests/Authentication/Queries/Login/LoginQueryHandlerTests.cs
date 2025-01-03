using Bogus;
using FluentAssertions;
using keycontrol.Application.Authentication.Common.Interfaces.Authentication;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Queries.Login;
using keycontrol.Application.Errors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using Xunit;

public class LoginQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IBcrypt> _bcrypt;
    private readonly LoginQueryHandler _loginQueryHandler;
    private readonly Faker _faker = new Faker("pt_BR");
    public LoginQueryHandlerTests(){
        _userRepositoryMock = new Mock<IUserRepository>();
        _bcrypt = new Mock<IBcrypt>();
        var _tokenJwtGenerator = new Mock<ITokenJwtGenerator>();
        var _tokenRepository = new Mock<ITokenRepository>(); 

        _loginQueryHandler = new LoginQueryHandler(_userRepositoryMock.Object, _bcrypt.Object, _tokenJwtGenerator.Object, _tokenRepository.Object);
    }

    [Fact]
    [Trait("Category", "LoginQueryHandler")]
    public async Task Handle_GivenUserNotRegistered_ThenReturnUserNotRegisteredErrorAsync()
    {
        var emailNotRegistered = _faker.Person.Email;
        var loginQuery = new LoginQuery(emailNotRegistered, _faker.Random.AlphaNumeric(8));
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(emailNotRegistered)).ReturnsAsync((User)null);

        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        var expectedResponseError = new UserNotRegistered("User not registered");

        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
    }

    [Fact]
    [Trait("Category", "LoginQueryHandler")]	
    public async Task Handle_GivenInvalidPassword_ThenReturnInvalidPasswordAsync()
    {
        var invalidPassword = _faker.Random.AlphaNumeric(8);
        var correctPassword = _faker.Random.AlphaNumeric(8);
        var loginQuery = new LoginQuery(_faker.Person.Email, invalidPassword);
        var user = new User(_faker.Person.FirstName, loginQuery.Email, correctPassword);
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(loginQuery.Email)).ReturnsAsync(user);
        _bcrypt.Setup(b => b.VerifyPassword(invalidPassword, user.Password)).Returns(false);

        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        var expectedResponseError = new InvalidPassword("Invalid password");

        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
    }
}