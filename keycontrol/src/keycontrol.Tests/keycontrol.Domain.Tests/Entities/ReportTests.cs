using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Domain.Enums;
using keycontrol.Domain.ValueObjects;
using keycontrol.Tests.Fakers;
using Xunit;

namespace keycontrol.Tests.Entities;

public class ReportTests
{
private readonly Faker _faker = new Faker("pt_BR");
    [Fact]
    public void Create_GivenValidReportsData_ThenCreateReport()
    {
        var email =  Email.Create(_faker.Person.Email);
        var user = User.Create(_faker.Person.UserName, email.Value, ValidPassword.Generate());
        var keyRoom = KeyRoom.Create(_faker.Random.Number(1, 100), _faker.Lorem.Word());

        var result = Report.Create(user.Value, keyRoom.Value);

        result.IsSuccess.Should().BeTrue();
        result.Value.IdUser.Should().Be(user.Value.Id);
        result.Value.IdKey.Should().Be(keyRoom.Value.Id);
        result.Value.Status.Should().Be(Status.Unavailable);
    }
}
