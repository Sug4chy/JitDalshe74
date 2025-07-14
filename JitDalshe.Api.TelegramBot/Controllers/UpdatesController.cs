using JitDalshe.Application.TelegramBot;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace JitDalshe.Api.TelegramBot.Controllers;

[ApiController]
[Route("/telegram-bot/v1/[controller]")]
public sealed class UpdatesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> HandleUpdate(
        [FromBody] Update update,
        [FromServices] UpdatesHandler handler,
        CancellationToken ct = default)
    {
        await handler.HandleAsync(update, ct);

        return Ok();
    }
}