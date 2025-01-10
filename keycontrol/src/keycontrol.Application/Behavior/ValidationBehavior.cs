using FluentValidation;
using keycontrol.Application.Errors.Validators;
using MediatR;
using OneOf;

namespace keycontrol.Application.Behavior;


public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IOneOf
{
    private readonly IValidator<TRequest> _validator;

    public ValidationBehavior(IValidator<TRequest> validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid)
        {   
            return await next();
        }

        var errors = validationResult.Errors.ConvertAll(validationFailure => new ValidationError()
        {
            Code = validationFailure.ErrorMessage,
            Description = validationFailure.PropertyName
        });
        return (dynamic) new ValidatorErrors(errors);
    }
}