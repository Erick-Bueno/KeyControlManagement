using keycontrol.Application.Errors.Validators;
using keycontrol.Domain.Enums;
using System.Collections.ObjectModel;

namespace keycontrol.Application.Errors;

public record AppError(string Detail, string NameError,  string ErrorType, IEnumerable<ValidationError>? ValidationErrors = null);
