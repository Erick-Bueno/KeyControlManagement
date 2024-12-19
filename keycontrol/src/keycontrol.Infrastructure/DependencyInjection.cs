using keycontrol.Application.Authentication.Common.Interfaces.Cryptography;
using keycontrol.Application.Repositories;
using keycontrol.Infrastructure.Cryptography;
using Microsoft.Extensions.DependencyInjection;

namespace keycontrol.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, IUserRepository>();
        services.AddScoped<IBcrypt, Bcrypt>();
        return services;
    }
}