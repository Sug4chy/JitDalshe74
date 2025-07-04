using JitDalshe.Domain.Entities.Reviews;

namespace JitDalshe.Application.Site.Dto;

public readonly record struct ReviewDto(
    string ReviewerName,
    int ReviewerAge,
    ReviewerStatus ReviewerStatus,
    string Text,
    DateOnly Date
);