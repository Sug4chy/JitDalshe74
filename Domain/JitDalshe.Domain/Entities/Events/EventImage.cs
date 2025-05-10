using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Events;

public sealed class EventImage : AuditableEntity<IdOf<EventImage>>
{
    public required string Url { get; init; }
    public required string ContentType { get; init; }

    public Event? Event { get; init; }
    public IdOf<Event> EventId { get; init; }
}