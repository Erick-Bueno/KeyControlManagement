using Bogus;
using FluentAssertions;
using keycontrol.Application.Errors;
using keycontrol.Application.Reports.Commands.RentKey;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using keycontrol.Tests.Extensions;
using keycontrol.Tests.Fakers;
using Moq;
using Xunit;

namespace keycontrol.Tests.Application.Reports.Commands.RentKey;
[Trait("Category", "RentKeyCommandHandler")]
public class RentKeyCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IKeyRepository> _keyRepositoryMock;
    private readonly Mock<IReportRepository> _reportRepositoryMock;
    private readonly RentKeyCommandHandler _rentKeyCommandHandler;
    private readonly Faker _faker = new Faker("pt_BR");

    public RentKeyCommandHandlerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _keyRepositoryMock = new Mock<IKeyRepository>();
        _reportRepositoryMock = new Mock<IReportRepository>();
        _rentKeyCommandHandler = new RentKeyCommandHandler(_userRepositoryMock.Object, _keyRepositoryMock.Object, _reportRepositoryMock.Object);
    }
    [Fact]
    public async Task Handle_GivenInvalidUserExternalId_ThenReturnUserNotRegisteredErrorAsync()
    {
        var invalidExternalIdUser = _faker.Random.Guid();
        var externalIdKey = _faker.Random.Guid();
        var rentKeyCommand = new RentKeyCommand(invalidExternalIdUser, externalIdKey);
        _userRepositoryMock.Setup(ur => ur.FindUserByExternalId(invalidExternalIdUser)).ReturnsAsync((User)null);

        var result = await _rentKeyCommandHandler.Handle(rentKeyCommand, CancellationToken.None);

        var expectedResponseError = new UserNotRegistered("User Not Registered");

        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);

    }
    [Fact]
    public async Task Handle_GivenUserBlocked_ThenReturnUserBlockedErrorAsync()
    {
        var externalIdUser = _faker.Random.Guid();
        var externalIdKey = _faker.Random.Guid();
        var rentKeyCommand = new RentKeyCommand(externalIdUser, externalIdKey);
        var email = Email.Create(_faker.Person.Email);
        var user = User.Create(_faker.Person.FullName, email.Value, ValidPassword.Generate());
        user.BlockUser();
        _userRepositoryMock.Setup(ur => ur.FindUserByExternalId(externalIdUser)).ReturnsAsync(user.Value);

        var result = await _rentKeyCommandHandler.Handle(rentKeyCommand, CancellationToken.None);

        var expectedResponseError = new UserBlocked("This User Is Blocked");

        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);

    }
    [Fact]
    public async Task Handle_GivenUnavailableKey_ThenReturnKeyUnavailableErrorAsync()
    {
        var externalIdUser = _faker.Random.Guid();
        var externalIdKey = _faker.Random.Guid();
        var rentKeyCommand = new RentKeyCommand(externalIdUser, externalIdKey);
        var email = Email.Create(_faker.Person.Email);
        var user = User.Create(_faker.Person.FullName, email.Value, ValidPassword.Generate());

        _userRepositoryMock.Setup(ur => ur.FindUserByExternalId(externalIdUser)).ReturnsAsync(user.Value);
        _keyRepositoryMock.Setup(kr => kr.FindKeyByExternalId(externalIdKey)).ReturnsAsync((KeyRoom)null);

        var result = await _rentKeyCommandHandler.Handle(rentKeyCommand, CancellationToken.None);

        var expectedResponseError = new UserBlocked("This Key Is Unavailable");

        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
    }
}