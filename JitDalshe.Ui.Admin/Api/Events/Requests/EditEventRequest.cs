using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Api.Events.Requests;

public sealed record EditEventRequest(
    string Title,
    string ShortDescription,
    string FullText,
    DateOnly? Date,
    TimeOnly? Time,
    string? Location,
    EventStatus Status
);