using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Events;

public sealed class EventPhoto : AuditableEntity<IdOf<EventPhoto>>
{
    public required string Url { get; init; }

    public Event? Event { get; init; }
    public IdOf<Event> EventId { get; init; }
}