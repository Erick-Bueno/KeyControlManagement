using keycontrol.Application.Errors.Validators;
using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record AppError(string Detail, string NameError,  string ErrorType, List<ValidationError>? ValidationErrors = null);
