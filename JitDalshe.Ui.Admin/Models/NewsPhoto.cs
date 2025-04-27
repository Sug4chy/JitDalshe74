namespace JitDalshe.Ui.Admin.Models;

public readonly record struct NewsPhoto(
    Guid Id,
    string Uri,
    bool IsPrimary
);