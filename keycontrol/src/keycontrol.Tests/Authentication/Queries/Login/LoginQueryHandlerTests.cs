using Bogus;
using FluentAssertions;
using keycontrol.Application.Authentication.Common.Interfaces.Authentication;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Queries.Login;
using keycontrol.Application.Authentication.Responses;
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
    private readonly Mock<ITokenRepository> _tokenRepository;
    private readonly Mock<ITokenJwtGenerator> _tokenJwtGenerator;
    private readonly Faker _faker = new Faker("pt_BR");
    public LoginQueryHandlerTests(){
        _userRepositoryMock = new Mock<IUserRepository>();
        _bcrypt = new Mock<IBcrypt>();
        _tokenJwtGenerator = new Mock<ITokenJwtGenerator>();
        _tokenRepository = new Mock<ITokenRepository>(); 

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

    [Fact]
    [Trait("Category", "LoginQueryHandler")]
    public async Task Handle_GivenTokenIsNotNull_ThenUpdateTokenAndLoginAsync()
    {
        var loginQuery = new LoginQuery(_faker.Person.Email, _faker.Random.AlphaNumeric(8));
        var user = new User(_faker.Person.FirstName, loginQuery.Email, loginQuery.Password);
        var accessToken = _faker.Random.AlphaNumeric(8);
        var refreshToken = _faker.Random.AlphaNumeric(8);
        var token = new Token(user.Email, refreshToken);
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(loginQuery.Email)).ReturnsAsync(user);
        _bcrypt.Setup(b => b.VerifyPassword(loginQuery.Password, user.Password)).Returns(true);
        _tokenRepository.Setup(t => t.FindTokenByEmail(user.Email)).ReturnsAsync(token);
        _tokenJwtGenerator.Setup(t => t.GenerateAccessToken(user.ExternalId)).Returns(accessToken);
        _tokenJwtGenerator.Setup(t => t.GenerateRefreshToken()).Returns(refreshToken);
        
        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);
        
        var expectedResponseSuccess= new LoginResponse(user.ExternalId, user.Name, user.Email, accessToken, refreshToken);
        
        _tokenRepository.Verify(t => t.UpdateToken(token, token.RefreshToken), Times.Once);
        result.AsT0.Email.Should().Be(expectedResponseSuccess.Email);
        result.AsT0.Name.Should().Be(expectedResponseSuccess.Name);
        result.AsT0.ExternalId.Should().Be(expectedResponseSuccess.ExternalId);
        result.AsT0.AccessToken.Should().Be(expectedResponseSuccess.AccessToken);
        result.AsT0.RefreshToken.Should().Be(expectedResponseSuccess.RefreshToken);
    }

    [Fact]
    [Trait("Category", "LoginQueryHandler")]
    public async Task Handle_GivenTokenIsNull_ThenCreateTokenAndLoginAsync()
    {
        var loginQuery = new LoginQuery(_faker.Person.Email, _faker.Random.AlphaNumeric(8));
        var user = new User(_faker.Person.FirstName, loginQuery.Email, loginQuery.Password);
        var accessToken = _faker.Random.AlphaNumeric(8);
        var refreshToken = _faker.Random.AlphaNumeric(8);
        var token = new Token(user.Email, refreshToken);
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(loginQuery.Email)).ReturnsAsync(user);
        _bcrypt.Setup(b => b.VerifyPassword(loginQuery.Password, user.Password)).Returns(true);
        _tokenRepository.Setup(t => t.FindTokenByEmail(user.Email)).ReturnsAsync((Token)null);
        _tokenJwtGenerator.Setup(t => t.GenerateAccessToken(user.ExternalId)).Returns(accessToken);
        _tokenJwtGenerator.Setup(t => t.GenerateRefreshToken()).Returns(refreshToken);
        
        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);
        
        var expectedResponseSuccess= new LoginResponse(user.ExternalId, user.Name, user.Email, accessToken, refreshToken);
        
        _tokenRepository.Verify(t => t.AddToken(It.IsAny<Token>()), Times.Once);
        result.AsT0.Email.Should().Be(expectedResponseSuccess.Email);
        result.AsT0.Name.Should().Be(expectedResponseSuccess.Name);
        result.AsT0.ExternalId.Should().Be(expectedResponseSuccess.ExternalId);
        result.AsT0.AccessToken.Should().Be(expectedResponseSuccess.AccessToken);
        result.AsT0.RefreshToken.Should().Be(expectedResponseSuccess.RefreshToken);
    }
}