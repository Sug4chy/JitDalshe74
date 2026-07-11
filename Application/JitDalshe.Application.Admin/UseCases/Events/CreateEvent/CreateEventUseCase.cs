using CSharpFunctionalExtensions;
using Ganss.Xss;
using JitDalshe.Application.Abstractions.ImageStorage;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Events.CreateEvent;

[UseCase]
internal sealed class CreateEventUseCase(IImageStorage imageStorage, IEventsRepository events, string imageUrlTemplate)
    : ICreateEventUseCase
{
    public async Task<UnitResult<Error>> CreateAsync(
        string title,
        string shortDescription,
        string fullText,
        DateOnly? date,
        TimeOnly? time,
        string? location,
        EventStatus status,
        string imageBase64Url,
        CancellationToken ct = default)
    {
        try
        {
            string imageContentType = imageBase64Url[(imageBase64Url.IndexOf(':') + 1)..imageBase64Url.IndexOf(';')];
            string imageContentString = imageBase64Url.Split(',')[1];
            byte[] imageBytes = Convert.FromBase64String(imageContentString);
            var imageId = await imageStorage.SaveImageAsync<EventImage>(imageBytes, imageContentType, ct);
            var eventId = IdOf<Event>.New();

            var eventImage = EventImage.Create(
                id: imageId,
                url: imageUrlTemplate.Replace("[id]", eventId.ToString()).Replace("[entity]", "events"),
                contentType: imageContentType,
                eventId: eventId);
            
            var sanitizer = new HtmlSanitizer();
            var sanitizedFullText = sanitizer.Sanitize(fullText);
            
            var @event = Event.Create(
                id: eventId,
                image: eventImage,
                title: title,
                shortDescription: shortDescription,
                fullText: sanitizedFullText,
                date: date,
                time: time,
                location: location,
                status: status
                );
            
            await events.AddAsync(@event, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}