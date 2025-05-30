using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Exceptions;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.ReplaceEventImage;

[UseCase]
internal sealed class ReplaceEventImageUseCase : IReplaceEventImageUseCase
{
    private readonly IEventsRepository _events;
    private readonly IImageStorage _imageStorage;

    public ReplaceEventImageUseCase(IEventsRepository events, IImageStorage imageStorage)
    {
        _events = events;
        _imageStorage = imageStorage;
    }

    public async Task<UnitResult<Error>> ReplaceAsync(
        IdOf<Event> eventId,
        string imageBase64Url,
        CancellationToken ct = default)
    {
        try
        {
            string imageContentType = imageBase64Url[(imageBase64Url.IndexOf(':') + 1)..imageBase64Url.IndexOf(';')];
            string imageContentString = imageBase64Url.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(imageContentString);

            var maybeEvent = await _events.FindByIdAsync(eventId, ct);
            if (maybeEvent.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Событие не найдено", ErrorGroup.NotFound));
            }

            var @event = maybeEvent.Value;

            await _imageStorage.RemoveImageAsync(@event.Image!.Id, ct);

            var newImageId = await _imageStorage.SaveImageAsync<EventImage>(imageBytes, imageContentType, ct);
            var newEventImage = new EventImage(newImageId)
            {
                Url = @event.Image!.Url,
                ContentType = imageBase64Url[(imageBase64Url.IndexOf(':') + 1)..imageBase64Url.IndexOf(';')],
                Event = @event,
                EventId = @event.Id
            };

            await _events.ReplaceEventImageAsync(@event, newEventImage, ct);

            return UnitResult.Success<Error>();
        }
        catch (ImageNotFoundException e)
        {
            return UnitResult.Failure(Error.Of(e.Message, ErrorGroup.NotFound));
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}