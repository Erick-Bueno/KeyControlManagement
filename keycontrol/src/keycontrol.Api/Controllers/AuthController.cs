using keycontrol.Application.Authentication.Commands.Register;
using keycontrol.Application.Authentication.Queries.Login;
using keycontrol.Application.Authentication.Requests;
using keycontrol.Application.Extension;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace keycontrol.Api.Controllers;
public class AuthController : ApiController
{
    private readonly ISender _sender;
    private readonly IConfiguration  _configuration;
    public AuthController(ISender sender, IConfiguration configuration)
    {
        _sender = sender;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var loginQuery = new LoginQuery(loginRequest.Email, loginRequest.Password);
        var result = await _sender.Send(loginQuery);
        return this.HandleResponseBase(result);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        var registerCommand = new RegisterCommand(registerRequest.Name,registerRequest.Email, registerRequest.Password);
        var result = await _sender.Send(registerCommand);
        return this.HandleResponseBase(result,new Uri(_configuration["BaseUri"]));
    }
}