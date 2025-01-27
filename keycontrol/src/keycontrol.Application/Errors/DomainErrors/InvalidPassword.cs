using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors.DomainErrors;

public record InvalidPassword(string Detail) : AppError(Detail, nameof(InvalidPassword), TypeError.BadRequest.ToString());