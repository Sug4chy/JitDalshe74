using JitDalse.Domain.Abstractions;
using JitDalse.Domain.ValueObjects;

namespace JitDalse.Domain.Entities;

public sealed class NewsPhoto : AuditableEntity<IdOf<NewsPhoto>>
{
    public long ExtId { get; }
    public Uri Uri { get; }

    public IdOf<News> NewsId { get; }
    public News? News { get; }

    public NewsPrimaryPhoto? PrimaryPhoto { get; }
}