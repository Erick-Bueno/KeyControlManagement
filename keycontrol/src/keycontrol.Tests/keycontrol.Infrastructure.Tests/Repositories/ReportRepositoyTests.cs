using Bogus;
using FluentAssertions;
using keycontrol.Domain.Entities;
using keycontrol.Domain.ValueObjects;
using keycontrol.Infrastructure.Repositories;
using keycontrol.Tests.Extensions;
using keycontrol.Tests.Fakers;
using keycontrol.Tests.Helpers;
using System.Reflection;
using Xunit;

namespace keycontrol.Tests.Repositories;
[Trait("Category", "ReportRepository")]
public class ReportRepositoyTests : DatabaseUnitTest
{
    private readonly ReportRepository _reportRepository;
    private readonly Faker _faker = new Faker("pt_BR");
    public ReportRepositoyTests()
    {
        _reportRepository = new ReportRepository(_dbContext);
    }
    [Fact]
    public async Task AddReport_GivenValidReport_ThenCreateReportAsync()
    {
        var email = Email.Create(_faker.Person.Email);
        var user = User.Create(_faker.Person.FullName, email.Value, ValidPassword.Generate());
        var key = KeyRoom.Create(_faker.Random.Number(1, 3), _faker.Lorem.Text());
        key.SetPrivateId(1);
        user.SetPrivateId(1);
  
        var report = Report.Create(user.Value, key.Value);
        await _reportRepository.AddReport(report.Value);
        var reportFound = await _dbContext.reports.FindAsync(report.Value.Id);

        reportFound.Should().NotBeNull();
        reportFound.Should().Be(report.Value);

    }
}
