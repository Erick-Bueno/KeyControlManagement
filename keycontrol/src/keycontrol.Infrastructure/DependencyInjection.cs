using keycontrol.Application.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace keycontrol.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, IUserRepository>();
        return services;
    }
}