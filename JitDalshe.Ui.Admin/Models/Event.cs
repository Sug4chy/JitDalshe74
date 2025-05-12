namespace JitDalshe.Ui.Admin.Models;

public sealed record Event(
    Guid Id,
    string Title,
    string? Description,
    DateOnly Date,
    string ImageUrl
);