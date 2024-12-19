using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record UserAlreadyRegistered(string Detail) : AppError(Detail, nameof(UserAlreadyRegistered), TypeError.Conflict.ToString());