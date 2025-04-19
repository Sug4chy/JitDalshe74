using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities;

public sealed class NewsPhoto : AuditableEntity<IdOf<NewsPhoto>>
{
    public long ExtId { get; init; }
    public Uri Uri { get; init; }

    public IdOf<News> NewsId { get; }
    public News? News { get; }

    public NewsPrimaryPhoto? PrimaryPhoto { get; }

    public NewsPhoto()
    {
        Id = IdOf<NewsPhoto>.New();
        CreatedAt = DateTime.UtcNow;
    }
}