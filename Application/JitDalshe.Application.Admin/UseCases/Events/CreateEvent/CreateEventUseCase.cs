using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.CreateEvent;

[UseCase]
internal sealed class CreateEventUseCase : ICreateEventUseCase
{
    private readonly IImageStorage _imageStorage;
    private readonly IEventsRepository _events;
    private readonly string _imageUrlTemplate;

    public CreateEventUseCase(IImageStorage imageStorage, IEventsRepository events, string imageUrlTemplate)
    {
        _imageStorage = imageStorage;
        _events = events;
        _imageUrlTemplate = imageUrlTemplate;
    }

    public async Task<UnitResult<Error>> CreateAsync(
        string title,
        string? description,
        DateTime date,
        string imageBase64Url,
        bool isDisplaying,
        CancellationToken ct = default)
    {
        try
        {
            string imageContentType = imageBase64Url[(imageBase64Url.IndexOf(':') + 1)..imageBase64Url.IndexOf(';')];
            string imageContentString = imageBase64Url.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(imageContentString);
            var imageId = await _imageStorage.SaveImageAsync<EventImage>(imageBytes, imageContentType, ct);
            var eventId = IdOf<Event>.New();

            var eventImage = new EventImage(imageId)
            {
                ContentType = imageContentType,
                Url = _imageUrlTemplate.Replace("[id]", eventId.ToString()).Replace("[entity]", "events")
            };
            var @event = new Event(eventId)
            {
                Title = title,
                Description = description,
                Date = DateOnly.FromDateTime(date),
                Image = eventImage,
                IsDisplaying = isDisplaying
            };

            await _events.AddAsync(@event, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}