using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record AppError(string Detail, string NameError,  TypeError ErrorType, List<ValidationError>? ValidationErrors = null);