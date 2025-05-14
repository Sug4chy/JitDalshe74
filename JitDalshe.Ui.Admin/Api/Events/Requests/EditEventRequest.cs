namespace JitDalshe.Ui.Admin.Api.Events.Requests;

public sealed record EditEventRequest(
    string Title,
    string? Description,
    DateTime Date,
    string ImageBase64Url
);