namespace JitDalshe.Ui.Admin.Api.Events.Requests;

public sealed record CreateEventRequest(
    string Title,
    string? Description,
    DateTime Date,
    string ImageBase64Url,
    bool IsDisplaying
);