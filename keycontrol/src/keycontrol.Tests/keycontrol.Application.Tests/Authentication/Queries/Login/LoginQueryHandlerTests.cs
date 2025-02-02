using Bogus;
using FluentAssertions;
using keycontrol.Application.Authentication.Common.Interfaces.Authentication;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Queries.Login;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using keycontrol.Application.Errors.DomainErrors;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using keycontrol.Tests.Fakers;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Moq;
using System.Threading.Tasks;
using Xunit;
namespace keycontrol.Tests.Authentication.Queries.Login;
[Trait("Category", "LoginQueryHandler")]
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
    public async Task Handle_GivenUserNotRegistered_ThenReturnUserNotRegisteredErrorAsync()
    {
        var emailNotRegistered = _faker.Person.Email;
        var emailNotRegisteredValueObject = Email.Create(emailNotRegistered);
        var loginQuery = new LoginQuery(emailNotRegistered, _faker.Random.AlphaNumeric(8));
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(emailNotRegisteredValueObject.Value)).ReturnsAsync((User)null);

        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        var expectedResponseError = new UserNotRegistered("User not registered");

        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
    }

    [Fact]	
    public async Task Handle_GivenInvalidPassword_ThenReturnInvalidPasswordAsync()
    {
        var invalidPassword = _faker.Random.AlphaNumeric(8);
        var correctPassword = _faker.Random.AlphaNumeric(8);
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var loginQuery = new LoginQuery(email, invalidPassword);
        var user = User.Create(_faker.Person.FirstName, emailValueObject.Value, correctPassword);
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(emailValueObject.Value)).ReturnsAsync(user.Value);
        _bcrypt.Setup(b => b.VerifyPassword(invalidPassword, user.Value.Password)).Returns(false);

        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        var expectedResponseError = new InvalidPassword("Invalid password");

        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
    }

    [Fact]
    public async Task Handle_GivenTokenIsNotNull_ThenUpdateTokenAndLoginAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var loginQuery = new LoginQuery(_faker.Person.Email, ValidPassword.Generate());
        var user = User.Create(_faker.Person.FirstName, emailValueObject.Value, loginQuery.Password);
        var accessToken = _faker.Random.AlphaNumeric(8);
        var refreshToken = _faker.Random.AlphaNumeric(8);
        var token = Token.Create(user.Value.Email, refreshToken);
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(emailValueObject.Value)).ReturnsAsync(user.Value);
        _bcrypt.Setup(b => b.VerifyPassword(loginQuery.Password, user.Value.Password)).Returns(true);
        _tokenRepository.Setup(t => t.FindTokenByEmail(emailValueObject.Value)).ReturnsAsync(token);
        _tokenJwtGenerator.Setup(t => t.GenerateAccessToken(user.Value.ExternalId)).Returns(accessToken);
        _tokenJwtGenerator.Setup(t => t.GenerateRefreshToken()).Returns(refreshToken);
        
        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);
        
        var expectedResponseSuccess= new LoginResponse(user.Value.ExternalId, user.Value.Name, user.Value.Email.EmailValue, accessToken, refreshToken);
        
        _tokenRepository.Verify(t => t.UpdateToken(token, token.RefreshToken), Times.Once);
        result.AsT0.Email.Should().Be(expectedResponseSuccess.Email);
        result.AsT0.Name.Should().Be(expectedResponseSuccess.Name);
        result.AsT0.ExternalId.Should().Be(expectedResponseSuccess.ExternalId);
        result.AsT0.AccessToken.Should().Be(expectedResponseSuccess.AccessToken);
        result.AsT0.RefreshToken.Should().Be(expectedResponseSuccess.RefreshToken);
    }

    [Fact]
    public async Task Handle_GivenTokenIsNull_ThenCreateTokenAndLoginAsync()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var loginQuery = new LoginQuery(_faker.Person.Email, ValidPassword.Generate());
        var user = User.Create(_faker.Person.FirstName,  emailValueObject.Value, loginQuery.Password);
        var accessToken = _faker.Random.AlphaNumeric(8);
        var refreshToken = _faker.Random.AlphaNumeric(8);
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(emailValueObject.Value)).ReturnsAsync(user.Value);
        _bcrypt.Setup(b => b.VerifyPassword(loginQuery.Password, user.Value.Password)).Returns(true);
        _tokenRepository.Setup(t => t.FindTokenByEmail(user.Value.Email)).ReturnsAsync((Token)null);
        _tokenJwtGenerator.Setup(t => t.GenerateAccessToken(user.Value.ExternalId)).Returns(accessToken);
        _tokenJwtGenerator.Setup(t => t.GenerateRefreshToken()).Returns(refreshToken);
        
        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);
        
        var expectedResponseSuccess= new LoginResponse(user.Value.ExternalId, user.Value.Name, user.Value.Email.EmailValue, accessToken, refreshToken);
        
        _tokenRepository.Verify(t => t.AddToken(It.IsAny<Token>()), Times.Once);
        result.AsT0.Email.Should().Be(expectedResponseSuccess.Email);
        result.AsT0.Name.Should().Be(expectedResponseSuccess.Name);
        result.AsT0.ExternalId.Should().Be(expectedResponseSuccess.ExternalId);
        result.AsT0.AccessToken.Should().Be(expectedResponseSuccess.AccessToken);
        result.AsT0.RefreshToken.Should().Be(expectedResponseSuccess.RefreshToken);
    }
    [Fact]
    public async Task Handle_GivenInvalidEmailToDomain_ReturnErrorInvalidEmailAsync()
    {
        var invalidEmail = _faker.Random.AlphaNumeric(8);
        var loginQuery = new LoginQuery(invalidEmail, ValidPassword.Generate());
        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        result.AsT1.NameError.Should().Be("InvalidEmail");
        result.AsT1.ErrorType.Should().Be("BadRequest");
    }
    [Fact]
    public async Task Handle_GivenInvalidPasswordToDomain_ReturnErrorInvalidPassword()
    {
        var email = _faker.Person.Email;
        var emailValueObject = Email.Create(email);
        var invalidPassword = _faker.Random.AlphaNumeric(8);
        var loginQuery = new LoginQuery(email, invalidPassword);
        var user = User.Create(_faker.Person.FirstName,  emailValueObject.Value, loginQuery.Password);
        _userRepositoryMock.Setup(ur => ur.FindUserByEmail(emailValueObject.Value)).ReturnsAsync(user.Value);

        var result = await _loginQueryHandler.Handle(loginQuery, CancellationToken.None);

        result.AsT1.NameError.Should().Be("InvalidPassword");
        result.AsT1.ErrorType.Should().Be("BadRequest");
    }
}