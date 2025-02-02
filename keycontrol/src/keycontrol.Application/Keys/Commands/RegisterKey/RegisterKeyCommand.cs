using keycontrol.Application.Errors;
using keycontrol.Application.Keys.Responses;
using MediatR;
using OneOf;

namespace keycontrol.Application.Keys.Commands.RegisterKey;

public record RegisterKeyCommand(Guid ExternalIdRoom, string Description) : IRequest<OneOf<RegisterKeyResponse, AppError>>;
