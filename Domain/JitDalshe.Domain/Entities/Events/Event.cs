using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Events;

public sealed class Event : AuditableEntity<IdOf<Event>>
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public DateOnly Date { get; init; }

    public EventPhoto? Photo { get; init; }
}