using System.ComponentModel.DataAnnotations;
using keycontrol.Application.Authentication.Responses;
using keycontrol.Application.Errors;
using MediatR;
using OneOf;

namespace keycontrol.Application.Authentication.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<OneOf<LoginResponse ,AppError>>;
