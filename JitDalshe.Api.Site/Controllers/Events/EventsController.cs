using System.Net.Mime;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Site.Controllers.Events.Requests;
using JitDalshe.Application.Site.UseCases.GetEventImage;
using JitDalshe.Application.Site.UseCases.ListEvents;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.Events;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class EventsController : AbstractController
{
    /// <summary>
    /// Получение событий с пагинацией (от новых к старым)
    /// </summary>
    [HttpGet]
    [ValidateRequest]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListEvents(
        [FromQuery] ListEventsRequest request,
        [FromServices] IListEventsUseCase listEvents,
        CancellationToken ct = default)
    {
        var result = await listEvents.ListAsync(request.PageNumber, request.PageSize, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }

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
}