using JetBrains.Annotations;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.News;

public sealed class NewsPrimaryImage
{
    public IdOf<News> NewsId { get; init; }
    public News? News { get; init; }

    public IdOf<NewsImage> NewsImageId { get; init; }
    public NewsImage? NewsImage { get; init; }

    private NewsPrimaryImage(
        IdOf<News> newsId,
        IdOf<NewsImage> newsImageId)
    {
        NewsId = newsId;
        NewsImageId = newsImageId;
    }

    public static NewsPrimaryImage Create(
        IdOf<News> newsId, 
        IdOf<NewsImage> newsImageId) 
        => new(newsId, newsImageId);

    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private NewsPrimaryImage()
    {
    }
#pragma warning restore CS8618
}