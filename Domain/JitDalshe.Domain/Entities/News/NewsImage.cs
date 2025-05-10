using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.News;

public sealed class NewsImage : AuditableEntity<IdOf<NewsImage>>
{
    public long ExtId { get; init; }
    public Uri Url { get; init; }

    public IdOf<News> NewsId { get; }
    public News? News { get; }

    public NewsPrimaryImage? PrimaryPhoto { get; }

    public NewsImage()
    {
        Id = IdOf<NewsImage>.New();
    }
}