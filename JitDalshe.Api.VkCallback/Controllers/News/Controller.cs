using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.VkCallback.Abstractions;
using JitDalshe.Application.VkCallback.Events;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.VkCallback.Controllers.News;

[ApiController]
[Route("/vk-callback-api/v1/[controller]")]
public sealed class Controller : AbstractController
{
    [HttpPost]
    public async Task<IActionResult> WallPostNew(
        [FromBody] WallPostNewVkEvent @event,
        [FromServices] IVkEventHandler<WallPostNewVkEvent> handler,
        CancellationToken ct = default)
    {
        var result = await handler.HandleAsync(@event, ct);

        return result.IsSuccess
            ? Ok("ok")
            : Error(result.Error);
    }
}