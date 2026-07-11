using JitDalshe.Ui.Admin.Models.Common;

namespace JitDalshe.Ui.Admin.Models;

public sealed record AdminUser(
    Guid Id,
    string Email,
    UserRole Role,
    bool IsActive,
    DateOnly CreatedAt
);