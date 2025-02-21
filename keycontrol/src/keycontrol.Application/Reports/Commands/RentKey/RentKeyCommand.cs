using keycontrol.Application.Errors;
using keycontrol.Application.Reports.Responses;

using MediatR;
using OneOf;

namespace keycontrol.Application.Reports.Commands.RentKey;

public record RentKeyCommand(Guid ExternalIdUser, Guid ExternalIdKey) : IRequest<OneOf<RentKeyResponse, AppError>>;