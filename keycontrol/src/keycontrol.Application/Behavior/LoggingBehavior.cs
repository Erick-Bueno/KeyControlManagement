using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using MediatR;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;

namespace keycontrol.Application.Behavior;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
where TResponse : IOneOf
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
     private static readonly Action<ILogger, string, string ,Exception> CompletedRequest = LoggerMessage.Define<string, string>(LogLevel.Error,  new EventId(13, nameof(CompletedRequest)), "Completed request {RequestName} with {Error}");
    private static readonly Action<ILogger, string ,Exception> ProcessRequest = LoggerMessage.Define<string>(LogLevel.Information,  new EventId(14, nameof(ProcessRequest)), "Processing request {RequestName}");
    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        ProcessRequest(_logger, requestName, default!);
        var result = await next();
        if(result is TResponse){
            ProcessRequest(_logger, requestName, default!);
        }
        else if(result is AppError appError){
            CompletedRequest(_logger, requestName,  appError.Detail, default!);
        }
        return result;
    }
}



