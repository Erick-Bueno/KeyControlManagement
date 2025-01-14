using keycontrol.Application.Errors;
using keycontrol.Application.Key.Responses;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using MediatR;
using OneOf;

namespace keycontrol.Application.Key.Commands;

public class RegisterKeyCommandHandler : IRequestHandler<RegisterKeyCommand, OneOf<RegisterKeyResponse, AppError>>
{ 
    private readonly IRoomRepository _roomRepository;
    private readonly IKeyRepository _keyRepository;

    public RegisterKeyCommandHandler(IRoomRepository roomRepository, IKeyRepository keyRepository)
    {
        _roomRepository = roomRepository;
        _keyRepository = keyRepository;
    }

    public async Task<OneOf<RegisterKeyResponse, AppError>> Handle(RegisterKeyCommand request, CancellationToken cancellationToken)
    {
        var isValidRoom = await _roomRepository.GetRoomByExternalId(request.ExternalIdRoom);
        if (isValidRoom is null)
        {
            return new RoomNotFinded("Room not found");
        }

        var key = new KeyRoom(isValidRoom.Id, request.Description);
        await _keyRepository.AddKey(key);
        
        return new RegisterKeyResponse(key.ExternalId, isValidRoom.ExternalId, key.Description, isValidRoom.Name);
    }
}