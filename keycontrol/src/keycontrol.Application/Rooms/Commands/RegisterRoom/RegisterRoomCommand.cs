using keycontrol.Application.Errors;
using keycontrol.Application.Rooms.Responses;
using MediatR;
using OneOf;

namespace keycontrol.Application.Rooms.Commands.RegisterRoom;

public record RegisterRoomCommand(string Name) : IRequest<OneOf<RegisterRoomResponse, AppError>>;