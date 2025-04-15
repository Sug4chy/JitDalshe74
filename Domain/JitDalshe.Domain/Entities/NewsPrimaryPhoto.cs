using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities;

public sealed class NewsPrimaryPhoto
{
    public IdOf<News> NewsId { get; init; }
    public News? News { get; init; }

    public IdOf<NewsPhoto> NewsPhotoId { get; init; }
    public NewsPhoto? NewsPhoto { get; init; }
}