using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Admin.Dto;

public readonly record struct ReviewDto(
    Guid Id,
    string ReviewerName,
    int ReviewerAge,
    ReviewerStatus ReviewerStatus,
    string Text,
    ReviewStatus Status,
    DateOnly Date
);