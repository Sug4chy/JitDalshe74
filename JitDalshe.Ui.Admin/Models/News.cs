namespace JitDalshe.Ui.Admin.Models;

public readonly record struct News(
    Guid Id,
    string Text,
    NewsPhoto[] Photos
);