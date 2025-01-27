using keycontrol.Application.Errors;
using keycontrol.Application.Room.Responses;
using MediatR;
using OneOf;

namespace keycontrol.Application.Room.Commands;

public record RegisterRoomCommand(string Name) : IRequest<OneOf<RegisterRoomResponse, AppError>>;