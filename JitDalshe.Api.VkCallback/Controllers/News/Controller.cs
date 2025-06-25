using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.VkCallback;
using JitDalshe.Application.VkCallback.Events;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.VkCallback.Controllers.News;

[ApiController]
[Route("/vk-callback-api/v1/events")]
public sealed class Controller : AbstractController
{
    [HttpPost]
    public async Task<IActionResult> WallPostNew(
        [FromBody] VkEvent @event,
        [FromServices] VkEventsDispatcher dispatcher,
        CancellationToken ct = default)
    {
        await dispatcher.DispatchAsync(@event, ct);

        return Ok("ok");
    }
}