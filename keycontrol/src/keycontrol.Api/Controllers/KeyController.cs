using keycontrol.Application.Extension;
using keycontrol.Application.Keys.Commands.RegisterKey;
using keycontrol.Application.Keys.Requests;
using keycontrol.Domain.Enums;
using keycontrol.Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keycontrol.Api.Controllers;
public class KeyController : ApiController
{
    private readonly ISender _sender;
    public KeyController(ISender sender)
    {
        _sender = sender;
    }
    [HasPermission(Permission.Administrator)]  
    [HttpPost]
    public async Task<IActionResult> AddKey([FromBody] RegisterKeyRequest registerKeyRequest)
    {
        var registerKeyCommand = new RegisterKeyCommand(registerKeyRequest.ExternalIdRoom, registerKeyRequest.Description);
        var result = await _sender.Send(registerKeyCommand);
        return this.RegisterKeyResponseBase(result);
    }
}