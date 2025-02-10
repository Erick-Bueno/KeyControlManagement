using keycontrol.Application.Authentication.Common.Interfaces.Authentication;
using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Authentication.Common.Interfaces.Services;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using keycontrol.Infrastructure.Authentication;
using keycontrol.Infrastructure.Context;
using keycontrol.Infrastructure.Cryptography;
using keycontrol.Infrastructure.Repositories;
using keycontrol.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace keycontrol.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("default")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<IKeyRepository, KeyRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IBcrypt, Bcrypt>();
        services.AddSingleton<ITokenJwtGenerator, TokenJwtGenerator>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        return services;
    }
}