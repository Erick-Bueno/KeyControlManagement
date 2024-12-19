using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace keycontrol.Application.Authentication.Commands.Register;

public record RegisterCommand(string Username, string Email, string Password) : IRequest<OneOf<RegisterResponse, AppError>>;