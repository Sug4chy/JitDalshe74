using System.Net.Mime;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Site.Requests;
using JitDalshe.Application.Site.UseCases.Events.GetEvent;
using JitDalshe.Application.Site.UseCases.Events.ListEvents;
using JitDalshe.Application.UseCases.Events.GetEventImage;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.Events;

[ApiController]
[Route("/api-site/v1/[controller]")]
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
        [FromQuery] ListWithPaginationRequest request,
        [FromServices] IListEventsUseCase listEvents,
        CancellationToken ct = default)
    {
        var result = await listEvents.ListAsync(request.PageNumber, request.PageSize, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }
    
    /// <summary>
    /// Получение конкретного события по ID (только опубликованных)
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetEvent(
        [FromRoute] Guid id,
        [FromServices] IGetEventUseCase getEvent,
        CancellationToken ct = default)
    {
        var result = await getEvent.GetAsync(IdOf<Event>.From(id), ct);
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