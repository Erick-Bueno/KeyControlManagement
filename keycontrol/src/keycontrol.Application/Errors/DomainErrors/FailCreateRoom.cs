using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors.DomainErrors;

public record FailCreateRoom(string Detail) : AppError(Detail, nameof(FailCreateRoom), TypeError.BadRequest.ToString());