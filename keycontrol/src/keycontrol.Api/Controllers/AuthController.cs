using keycontrol.Application.Authentication.Queries.Login;
using keycontrol.Application.Authentication.Requests;
using keycontrol.Application.Extension;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace keycontrol.Api.Controllers;
public class AuthController : ApiController
{
    private readonly ISender _sender;
    public AuthController(ISender sender)
    {
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var loginQuery = new LoginQuery(loginRequest.Name, loginRequest.Password);
        var result = await _sender.Send(loginQuery);
        return this.LoginResponseBase(result);
    }
}