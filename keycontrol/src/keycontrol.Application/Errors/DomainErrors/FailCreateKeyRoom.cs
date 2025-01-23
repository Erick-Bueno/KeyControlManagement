using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors.DomainErrors;

public record FailCreateKeyRoom(string Detail) : AppError(Detail, nameof(FailCreateKeyRoom), TypeError.BadRequest.ToString());