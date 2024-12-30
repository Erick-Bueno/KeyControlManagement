using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace keycontrol.Application.Behavior;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse : OneOfBase<Success, AppError>
{
    private readonly ILogger _logger;

    public LoggingBehavior(ILogger logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Processing request {requestName}", requestName);
        var result = await next();
        if(result.IsT0){
            _logger.LogInformation("Completed request {requestName}", requestName);
        }
        else{
            _logger.LogError("Completed request {requestName} with {error}", requestName, result.AsT1.Detail);
        }
        return result;
    }
}