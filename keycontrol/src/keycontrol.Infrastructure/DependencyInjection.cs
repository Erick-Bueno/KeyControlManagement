using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Repositories;
using keycontrol.Infrastructure.Context;
using keycontrol.Infrastructure.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace keycontrol.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("default")));
        services.AddScoped<IUserRepository, IUserRepository>();
        services.AddScoped<IBcrypt, Bcrypt>();
        return services;
    }
}