namespace JitDalshe.Application.Site.Dto;

public readonly record struct NewsDto(
    string Text,
    string ImageUrl,
    string PostUrl
);