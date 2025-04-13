using JitDalse.Domain.ValueObjects;

namespace JitDalse.Domain.Entities;

public sealed class NewsPrimaryPhoto
{
    public IdOf<News> NewsId { get; }
    public News? News { get; }

    public IdOf<NewsPhoto> NewsPhotoId { get; }
    public NewsPhoto? NewsPhoto { get; }
}