using JitDalshe.Application.Errors;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Controllers.Base;

public abstract class AbstractController : ControllerBase
{
    [NonAction]
    protected IActionResult Error(Error error)
        => StatusCode((int)error.Group, new { error.Message });
}