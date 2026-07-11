using JitDalshe.Api.Models;
using JitDalshe.Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Controllers.Base;

[ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
public abstract class AbstractController : ControllerBase
{
    [NonAction]
    protected IActionResult Error(Error error)
        => StatusCode((int)error.Group, new { error.Message });
}