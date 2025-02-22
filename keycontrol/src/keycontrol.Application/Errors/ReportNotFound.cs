using keycontrol.Domain.Enums;

namespace keycontrol.Application.Errors;

public record ReportNotFound(string Detail) : AppError(Detail, nameof(ReportNotFound), TypeError.Conflict.ToString());