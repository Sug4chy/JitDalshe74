using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.EditEvent;

[UseCase]
internal sealed class EditEventUseCase : IEditEventUseCase
{
    private readonly IEventsRepository _events;
    private readonly IImageStorage _imageStorage;

    public EditEventUseCase(IEventsRepository events, IImageStorage imageStorage)
    {
        _events = events;
        _imageStorage = imageStorage;
    }

    public async Task<UnitResult<Error>> EditAsync(
        IdOf<Event> id,
        string title,
        string? description,
        DateTime date,
        string imageBase64Url,
        CancellationToken ct = default)
    {
        try
        {
            var maybeEvent = await _events.FindByIdAsync(id, ct);
            if (maybeEvent.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Событие не найдено", ErrorGroup.NotFound));
            }

            var @event = maybeEvent.Value;

            await _imageStorage.RemoveImageAsync(@event.Image!.Id, ct);

            var newImageId = await SaveNewImageAsync(imageBase64Url, ct);
            @event.Image = new EventImage(newImageId)
            {
                Url = @event.Image!.Url,
                ContentType = imageBase64Url[(imageBase64Url.IndexOf(':') + 1)..imageBase64Url.IndexOf(';')],
                Event = @event,
                EventId = @event.Id
            };
            @event.Title = title;
            @event.Description = description;
            @event.Date = DateOnly.FromDateTime(date);

            await _events.EditAsync(@event, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }

    private Task<IdOf<EventImage>> SaveNewImageAsync(string imageBase64Url, CancellationToken ct = default)
    {
        string imageContentType = imageBase64Url[(imageBase64Url.IndexOf(':') + 1)..imageBase64Url.IndexOf(';')];
        string imageContentString = imageBase64Url.Split(',')[1];
        byte[] imageBytes = Convert.FromBase64String(imageContentString);

        return _imageStorage.SaveImageAsync(imageBytes, imageContentType, ct);
    }
}