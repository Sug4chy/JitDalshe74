using System.Net.Mime;
using JitDalshe.Api.Admin.Controllers.Events.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Admin.UseCases.Events.CreateEvent;
using JitDalshe.Application.Admin.UseCases.Events.DeleteEvent;
using JitDalshe.Application.Admin.UseCases.Events.EditEvent;
using JitDalshe.Application.Admin.UseCases.Events.ListEvents;
using JitDalshe.Application.Admin.UseCases.Events.ReplaceEventImage;
using JitDalshe.Application.UseCases.Events.GetEventImage;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;
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

    /// <summary>
    /// Создание нового события
    /// </summary>
    [HttpPost]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    /// <summary>
    /// Редактирование события (заголовка, описания и даты)
    /// </summary>
    [HttpPatch("{id:guid}")]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditEvent(
        [FromRoute] Guid id,
        [FromBody] EditEventRequest request,
        [FromServices] IEditEventUseCase editEvent,
        CancellationToken ct = default)
    {
        var result = await editEvent.EditAsync(
            id: IdOf<Event>.From(id),
            title: request.Title,
            description: request.Description,
            date: request.Date,
            ct: ct);

        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }

    /// <summary>
    /// Изменение изображения, которое прикреплено к событию
    /// </summary>
    [HttpPatch("{id:guid}/image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReplaceEventImage(
        [FromRoute] Guid id,
        [FromBody] ReplaceEventImageRequest request,
        [FromServices] IReplaceEventImageUseCase replaceEventImage,
        CancellationToken ct = default)
    {
        var result = await replaceEventImage.ReplaceAsync(
            eventId: IdOf<Event>.From(id),
            imageBase64Url: request.ImageBase64Url,
            ct: ct);

        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }

    /// <summary>
    /// Удаление события
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteEvent(
        [FromRoute] Guid id,
        [FromServices] IDeleteEventUseCase deleteEvent,
        CancellationToken ct = default)
    {
        var result = await deleteEvent.DeleteAsync(IdOf<Event>.From(id), ct);

        return result.IsSuccess
            ? NoContent()
            : Error(result.Error);
    }
}