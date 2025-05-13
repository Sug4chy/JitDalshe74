using System.Net.Mime;
using JitDalshe.Api.Admin.Controllers.Events.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Admin.UseCases.CreateEvent;
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

    /// <summary>
    /// Получение полного списка событий (от новых к старым)
    /// </summary>
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

    [HttpPost]
    [ValidateRequest]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateEvent(
        [FromBody] CreateEventRequest request,
        [FromServices] ICreateEventUseCase createEvent,
        CancellationToken ct = default)
    {
        var result = await createEvent.CreateAsync(
            title: request.Title,
            description: request.Description,
            date: request.Date,
            imageBase64Url: request.ImageBase64Url,
            ct: ct);

        return result.IsSuccess
            ? CreatedAtAction("ListEvents", null)
            : Error(result.Error);
    }
}