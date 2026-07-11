using JitDalshe.Domain.Common;

namespace JitDalshe.Application.Admin.Dto;

public readonly record struct AdminUserDto(
    Guid Id,
    string Email,
    UserRole Role,
    bool IsActive,
    DateOnly CreatedAt
    );