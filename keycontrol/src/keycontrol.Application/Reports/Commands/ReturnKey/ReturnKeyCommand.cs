using keycontrol.Application.Errors;
using keycontrol.Application.Reports.Responses;
using MediatR;
using OneOf;

namespace keycontrol.Application.Reports.Commands.ReturnKey;

public record ReturnKeyCommand(Guid ExternalIdReport) : IRequest<OneOf<ReturnKeyResponse,AppError>>;