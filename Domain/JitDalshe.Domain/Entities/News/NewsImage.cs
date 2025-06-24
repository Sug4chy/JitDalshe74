using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.News;

public sealed class NewsImage : AuditableEntity<IdOf<NewsImage>>
{
    public long ExtId { get; init; }
    public Uri Url { get; init; }

    public IdOf<News> NewsId { get; init; }
    public News? News { get; init; }

    public NewsPrimaryImage? PrimaryPhoto { get; init; }

    private NewsImage(
        IdOf<NewsImage> id,
        long extId,
        Uri url)
    {
        Id = id;
        ExtId = extId;
        Url = url;
    }

    public static NewsImage Create(IdOf<NewsImage> id, long extId, Uri url) => new(id, extId, url);

    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private NewsImage()
    {
    }
#pragma warning restore CS8618
}