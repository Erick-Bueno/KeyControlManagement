using keycontrol.Application.Errors;
using keycontrol.Application.Errors.DomainErrors;
using keycontrol.Application.Repositories;
using keycontrol.Application.Rooms.Responses;
using keycontrol.Domain.Entities;
using MediatR;
using OneOf;

namespace keycontrol.Application.Rooms.Commands.RegisterRoom;

public class RegisterRoomCommandHandler : IRequestHandler<RegisterRoomCommand, OneOf<RegisterRoomResponse, AppError>>
{
    private readonly IRoomRepository _roomRepository;

    public RegisterRoomCommandHandler(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<OneOf<RegisterRoomResponse, AppError>> Handle(RegisterRoomCommand request, CancellationToken cancellationToken)
    {
       var newRoom = Room.Create(request.Name);
       if(newRoom.IsFailure){
            return new FailCreateKeyRoom(newRoom.ErrorMessage);
       }
       await _roomRepository.AddRoom(newRoom.Value);
       return new RegisterRoomResponse(newRoom.Value.ExternalId, newRoom.Value.Name);
    }
}