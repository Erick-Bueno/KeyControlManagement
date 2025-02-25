using keycontrol.Application.Errors;
using keycontrol.Application.Errors.DomainErrors;
using keycontrol.Application.Keys.Responses;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Entities;
using MediatR;
using OneOf;

namespace keycontrol.Application.Keys.Commands.RegisterKey;

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

        var key = KeyRoom.Create(isValidRoom.Id, request.Description);
        if(key is {IsFailure: true}) {
            return new FailCreateKeyRoom(key.ErrorMessage);
        }
        await _keyRepository.AddKey(key.Value);
        
        return new RegisterKeyResponse(key.Value.ExternalId, isValidRoom.ExternalId, key.Value.Description, isValidRoom.Name);
    }
}