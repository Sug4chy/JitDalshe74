using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Events;

public sealed class Event : AuditableEntity<IdOf<Event>>
{
    public required string Title { get; set; }
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public bool IsDisplaying { get; set; }

    public EventImage? Image { get; set; }

    public Event(IdOf<Event> id)
    {
        Id = id;
    }
}