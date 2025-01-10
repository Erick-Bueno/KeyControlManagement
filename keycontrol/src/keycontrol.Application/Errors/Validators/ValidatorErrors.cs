using keycontrol.Domain.Enums;
using System.Collections.ObjectModel;

namespace keycontrol.Application.Errors.Validators;


public record ValidatorErrors(IEnumerable<ValidationError> errors)
    : AppError("Validation errors" ,nameof(ValidatorErrors), TypeError.ValidationError.ToString(), errors);