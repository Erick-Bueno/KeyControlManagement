using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record KeyNotFound(string Detail) : AppError(Detail, nameof(KeyNotFound), TypeError.Conflict.ToString());