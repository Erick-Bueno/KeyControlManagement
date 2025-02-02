using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record IncorrectPassword(string Detail) : AppError(Detail, nameof(IncorrectPassword), TypeError.Conflict.ToString());