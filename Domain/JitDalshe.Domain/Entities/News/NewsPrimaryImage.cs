using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.News;

public sealed class NewsPrimaryImage
{
    public IdOf<Entities.News.News> NewsId { get; init; }
    public Entities.News.News? News { get; init; }

    public IdOf<NewsImage> NewsImageId { get; init; }
    public NewsImage? NewsImage { get; init; }
}