using keycontrol.Application.Errors;
using keycontrol.Application.Key.Responses;
using MediatR;
using OneOf;

namespace keycontrol.Application.Key.Commands;

public record RegisterKeyCommand(Guid ExternalIdRoom, string Description) : IRequest<OneOf<RegisterKeyResponse, AppError>>;
