using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Admin.Dto;

public readonly record struct UnmoderatedReviewDto(
    Guid Id,
    string ReviewerName,
    int ReviewerAge,
    ReviewerStatus ReviewerStatus,
    string Text
);