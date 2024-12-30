using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record InvalidPassword(string Detail) : AppError(Detail, nameof(InvalidPassword), TypeError.Conflict.ToString());