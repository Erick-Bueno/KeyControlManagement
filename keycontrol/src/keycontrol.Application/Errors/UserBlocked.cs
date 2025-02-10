using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record UserBlocked(string Detail) : AppError(Detail, nameof(UserBlocked), TypeError.Conflict.ToString());