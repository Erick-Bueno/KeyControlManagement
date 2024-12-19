using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors.Validators;


public record ValidatorErrors(List<ValidationError> errors)
    : AppError("Validation errors" ,nameof(ValidatorErrors), TypeError.ValidationError.ToString(), errors);