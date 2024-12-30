using System.Reflection;
using FluentValidation;
using keycontrol.Application.Behavior;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace keycontrol.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>)); //registrar o pipeline de validação que e executado antes de chamar o handler 
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>)); //registrar o pipeline de log
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly()); //registrar os validadores
        return services;
    }
}