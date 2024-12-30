using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record UserNotRegistered(string Detail) : AppError(Detail, nameof(UserNotRegistered), TypeError.Conflict.ToString());
