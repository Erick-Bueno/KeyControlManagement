using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace keycontrol.Api.Controllers;

[ApiController]
[Route("api/v{apiVersion:apiVersion}/[controller]")] 
public class ApiController : ControllerBase
{
}