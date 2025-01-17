using keycontrol.Domain;
using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors.DomainErrors;

public record InvalidEmail(string Detail) : AppError(Detail, nameof(InvalidEmail), TypeError.BadRequest.ToString());