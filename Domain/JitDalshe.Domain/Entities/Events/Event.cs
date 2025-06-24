using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Events;

public sealed class Event : AuditableEntity<IdOf<Event>>
{
    public string Title { get; set; }
    public string? Description { get; set; }
    public DateOnly Date { get; set; }
    public bool IsDisplaying { get; set; }

    public EventImage? Image { get; set; }

    private Event(
        IdOf<Event> id,
        string title,
        string? description,
        DateOnly date,
        bool isDisplaying,
        EventImage? image = null)
    {
        Id = id;
        Title = title;
        Description = description;
        Date = date;
        IsDisplaying = isDisplaying;
        Image = image;
    }

    public static Event Create(
        IdOf<Event> id,
        string title,
        string? description,
        DateOnly date,
        bool isDisplaying,
        EventImage? image = null)
        => new(id, title, description, date, isDisplaying, image);

    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private Event()
    {
    }
#pragma warning restore CS8618
}