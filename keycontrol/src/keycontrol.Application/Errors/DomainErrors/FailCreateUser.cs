using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors.DomainErrors;

public record FailCreateUser(string Detail) : AppError(Detail, nameof(FailCreateUser), TypeError.BadRequest.ToString());