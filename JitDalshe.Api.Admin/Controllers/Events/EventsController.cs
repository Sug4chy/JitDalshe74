using System.Net.Mime;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Admin.UseCases.ListEvents;
using JitDalshe.Application.UseCases.GetEventImage;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Events;

[ApiController]
[Route("/admin-api/v1/[controller]")]
public sealed class EventsController : AbstractController
{
    /// <summary>
    /// Получение изображения, привязанного к событию
    /// </summary>
    [HttpGet("{id:guid}/image")]
    [Produces(MediaTypeNames.Multipart.FormData)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEventImage(
        [FromRoute] Guid id,
        [FromServices] IGetEventImageUseCase getEventImage,
        CancellationToken ct = default)
    {
        var result = await getEventImage.GetImageAsync(id, ct);

        return result.IsSuccess
            ? File(result.Value.ImageStream, result.Value.ContentType)
            : Error(result.Error);
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListEvents(
        [FromServices] IListEventsUseCase listEvents,
        CancellationToken ct = default)
    {
        var result = await listEvents.ListAsync(ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }
}