using keycontrol.Application.Extension;
using keycontrol.Application.Rooms.Commands.RegisterRoom;
using keycontrol.Application.Rooms.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace keycontrol.Api.Controllers;

public class RoomController : ApiController
{
    private readonly ISender _sender;

    private readonly IConfiguration _configuration;

    public RoomController(ISender sender, IConfiguration configuration)
    {
        _sender = sender;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> AddRoom([FromBody] RegisterRoomRequest registerRoomRequest)
    {
        var registerRoomCommand = new RegisterRoomCommand(registerRoomRequest.Name);
        var result = await _sender.Send(registerRoomCommand);
        return this.HandleResponseBase(result, new Uri(_configuration["BaseUri"]));
    }
}