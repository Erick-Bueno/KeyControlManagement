using keycontrol.Application.Errors;
using keycontrol.Application.Reports.Responses;
using keycontrol.Application.Repositories;
using keycontrol.Domain.Enums;
using MediatR;
using OneOf;

namespace keycontrol.Application.Reports.Commands.ReturnKey;

public class ReturnKeyCommandHandler : IRequestHandler<ReturnKeyCommand, OneOf<ReturnKeyResponse, AppError>>
{
    private readonly IUserRepository _userRepository;
    private readonly IKeyRepository _keyRepository;
    public ReturnKeyCommandHandler(IUserRepository userRepository, IKeyRepository keyRepository)
    {
        _userRepository = userRepository;
        _keyRepository = keyRepository;
    }

    public async Task<OneOf<ReturnKeyResponse, AppError>> Handle(ReturnKeyCommand request, CancellationToken cancellationToken)
    {
    /*     var userFound = await _userRepository.FindUserByExternalId(request.ExternalIdUser);
        if (userFound is null)
        {
            return new UserNotRegistered("User not registered");
        }
        var keyFound = await _keyRepository.FindKeyByExternalId(request.ExternalIdKey);
        if (keyFound is null)
        {
            return new KeyNotFound("Key not found");
        }
        keyFound.UpdateStatus(Status.Available);
        if(userFound.Blocked){
            userFound.UpdateStatus(false);
        } */
        //atualizar o usuario
        //atualizar a chave
        //atualizar o report 
    }
}