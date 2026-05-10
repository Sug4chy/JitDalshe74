using System.Text.Json.Serialization;

namespace JitDalshe.Ui.Admin.Models;

public sealed record Event(
    Guid Id,
    string Title,
    string ShortDescription,
    string FullText,
    DateOnly? Date,
    TimeOnly? Time,
    string? Location,
    EventStatus Status,
    string ImageUrl
    )
{
    public string DisplayingStatus => Status == EventStatus.NotPublished ? "Не опубликовано" : "Опубликовано";
    public bool IsDisplaying => Status == EventStatus.Published;
}

public enum EventStatus
{
    NotPublished,
    Published
}