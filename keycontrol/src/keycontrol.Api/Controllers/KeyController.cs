using keycontrol.Application.Extension;
using keycontrol.Application.Key.Commands;
using keycontrol.Application.Key.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace keycontrol.Api.Controllers;

public class KeyController : ApiController
{
    private readonly ISender _sender;
    public KeyController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> AddKey([FromBody] RegisterKeyRequest registerKeyRequest)
    {
        var registerKeyCommand = new RegisterKeyCommand(registerKeyRequest.ExternalIdRoom, registerKeyRequest.Description);
        var result = await _sender.Send(registerKeyCommand);
        return this.RegisterKeyResponseBase(result);
    }
}