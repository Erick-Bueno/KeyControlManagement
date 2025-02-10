using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record KeyUnavailable(string Detail) : AppError(Detail, nameof(KeyUnavailable), TypeError.Conflict.ToString());