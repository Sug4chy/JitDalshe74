using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.News;

public sealed class NewsPrimaryPhoto
{
    public IdOf<Entities.News.News> NewsId { get; init; }
    public Entities.News.News? News { get; init; }

    public IdOf<NewsPhoto> NewsPhotoId { get; init; }
    public NewsPhoto? NewsPhoto { get; init; }
}