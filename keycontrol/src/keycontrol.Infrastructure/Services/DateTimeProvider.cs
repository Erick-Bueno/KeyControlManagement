using keycontrol.Application.Authentication.Common.Interfaces.Services;
namespace keycontrol.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}