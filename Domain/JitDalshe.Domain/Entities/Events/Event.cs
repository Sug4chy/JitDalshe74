using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Events;

public sealed class Event : AuditableEntity<IdOf<Event>>
{
    public EventImage Image { get; private set; }
    public string Title { get; private set; }
    public string ShortDescription { get; private set; }
    public string FullText { get; private set; }
    public DateOnly? Date { get; private set; }
    public TimeOnly? Time { get; private set; }
    public string? Location { get; private set; }
    public EventStatus Status { get; private set; }

    private Event(
        IdOf<Event> id,
        EventImage image,
        string title,
        string shortDescription,
        string fullText,
        DateOnly? date,
        TimeOnly? time,
        string? location, 
        EventStatus status)
    {
        Id = id;
        Image = image;
        Title = title;
        ShortDescription = shortDescription;
        FullText = fullText;
        Date = date;
        Time = time;
        Location = location;
        Status = status;
    }

    public static Event Create(
        IdOf<Event> id,
        EventImage image,
        string title,
        string shortDescription,
        string fullText,
        DateOnly? date,
        TimeOnly? time,
        string? location,
        EventStatus status = EventStatus.NotPublished)
        => new(id, image, title, shortDescription, fullText, date, time, location, status);


    public void UpdateEventDetails(
        string title, 
        string shortDescription, 
        string fullText, 
        DateOnly? date,
        TimeOnly? time,
        string? location)
    {
        Title = title;
        ShortDescription = shortDescription;
        FullText = fullText;
        Date = date;
        Time = time;
        Location = location;
    }
    
    public void ChangeEventStatus(EventStatus status) => Status = status;
    
    public void ReplaceImage(EventImage newImage) => Image = newImage;
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