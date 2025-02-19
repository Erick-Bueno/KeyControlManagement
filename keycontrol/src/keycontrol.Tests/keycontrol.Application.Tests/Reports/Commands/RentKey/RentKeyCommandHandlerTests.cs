using Bogus;
using FluentAssertions;
using keycontrol.Application.Errors;
using keycontrol.Application.Reports.Commands.RentKey;
using keycontrol.Application.Reports.Responses;
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
    public async Task Handle_GivenNotRegisteredKey_ThenReturnKeyNotFoundErrorAsync()
    {
        var externalIdUser = _faker.Random.Guid();
        var externalIdKey = _faker.Random.Guid();
        var rentKeyCommand = new RentKeyCommand(externalIdUser, externalIdKey);
        var email = Email.Create(_faker.Person.Email);
        var user = User.Create(_faker.Person.FullName, email.Value, ValidPassword.Generate());

        _userRepositoryMock.Setup(ur => ur.FindUserByExternalId(externalIdUser)).ReturnsAsync(user.Value);
        _keyRepositoryMock.Setup(kr => kr.FindKeyByExternalId(externalIdKey)).ReturnsAsync((KeyRoom)null);

        var result = await _rentKeyCommandHandler.Handle(rentKeyCommand, CancellationToken.None);

        var expectedResponseError = new KeyNotFound("Key not found");

        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);
    }
    [Fact]
    public async Task Handle_GivenUnavailableKey_ThenReturnUnavailableKeyErrorAsync()
    {
        var externalIdUser = _faker.Random.Guid();
        var externalIdKey = _faker.Random.Guid();
        var rentKeyCommand = new RentKeyCommand(externalIdUser, externalIdKey);
        var email = Email.Create(_faker.Person.Email);
        var user = User.Create(_faker.Person.FullName, email.Value, ValidPassword.Generate());
        var keyRoom = KeyRoom.Create(_faker.Random.Int(), _faker.Lorem.Text());
        keyRoom.SetStatus();

        _userRepositoryMock.Setup(ur => ur.FindUserByExternalId(externalIdUser)).ReturnsAsync(user.Value);
        _keyRepositoryMock.Setup(kr => kr.FindKeyByExternalId(externalIdKey)).ReturnsAsync(keyRoom.Value);

        var result = await _rentKeyCommandHandler.Handle(rentKeyCommand, CancellationToken.None);

        var expectedResponseError = new KeyUnavailable("This Key Is Unavailable");

        result.AsT1.Detail.Should().Be(expectedResponseError.Detail);
        result.AsT1.ErrorType.Should().Be(expectedResponseError.ErrorType);
        result.AsT1.NameError.Should().Be(expectedResponseError.NameError);


    }
    [Fact]
    public async Task Handle_GivenValidRentKeyData_ThenCreateReportAsync()
    {
        var externalIdUser = _faker.Random.Guid();
        var externalIdKey = _faker.Random.Guid();
        var rentKeyCommand = new RentKeyCommand(externalIdUser, externalIdKey);
        var email = Email.Create(_faker.Person.Email);
        var user = User.Create(_faker.Person.FullName, email.Value, ValidPassword.Generate());
        var keyRoom = KeyRoom.Create(_faker.Random.Int(), _faker.Lorem.Text());
        var report = Report.Create(user.Value, keyRoom.Value);
        _userRepositoryMock.Setup(ur => ur.FindUserByExternalId(externalIdUser)).ReturnsAsync(user.Value);
        _keyRepositoryMock.Setup(kr => kr.FindKeyByExternalId(externalIdKey)).ReturnsAsync(keyRoom.Value);
        _reportRepositoryMock.Setup(rr => rr.AddReport(report.Value));

        var result = await _rentKeyCommandHandler.Handle(rentKeyCommand, CancellationToken.None);

        var expectedResponseSuccess = new RentKeyResponse(user.Value.ExternalId, keyRoom.Value.ExternalId, user.Value.Name, report.Value.WithdrawalDate);

        result.AsT0.ExternalIdKey.Should().Be(expectedResponseSuccess.ExternalIdKey);
        result.AsT0.Name.Should().Be(expectedResponseSuccess.Name);
        result.AsT0.WithdrawalDate.Should().Be(expectedResponseSuccess.WithdrawalDate);
        result.AsT0.Message.Should().Be("Key rented successfully");
        result.AsT0.ExternalIdUser.Should().Be(expectedResponseSuccess.ExternalIdUser);
    }
}