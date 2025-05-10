namespace JitDalshe.Ui.Admin.Models;

public readonly record struct NewsImage(
    Guid Id,
    string Url,
    bool IsPrimary
);