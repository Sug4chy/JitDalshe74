using System.Text.Json.Serialization;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Api.Events.Requests;

public sealed record CreateEventRequest(
    string Title,
    string ShortDescription,
    string FullText,
    DateOnly? Date,
    TimeOnly? Time,
    string? Location,
    EventStatus Status,
    string ImageBase64Url
);