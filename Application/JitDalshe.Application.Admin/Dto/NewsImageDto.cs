namespace JitDalshe.Application.Admin.Dto;

public readonly record struct NewsImageDto(
    Guid Id,
    string Url,
    bool IsPrimary
);