using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace keycontrol.Api.Controllers;

public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult Error()
    {
        var httpException = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;
        return Problem();
    }
}