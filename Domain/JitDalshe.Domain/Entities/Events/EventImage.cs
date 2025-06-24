using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Events;

public sealed class EventImage : AuditableEntity<IdOf<EventImage>>, IImage
{
    public string Url { get; init; }
    public string ContentType { get; init; }

    public Event? Event { get; init; }
    public IdOf<Event> EventId { get; init; }

    private EventImage(
        IdOf<EventImage> id,
        string url,
        string contentType,
        IdOf<Event> eventId,
        Event? @event = null)
    {
        Id = id;
        Url = url;
        ContentType = contentType;
        Event = @event;
        EventId = eventId;
    }

    public static EventImage Create(
        IdOf<EventImage> id, 
        string url, 
        string contentType, 
        IdOf<Event> eventId, 
        Event? @event = null) 
        => new(id, url, contentType, eventId, @event);

    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private EventImage()
    {
    }
#pragma warning restore CS8618
}