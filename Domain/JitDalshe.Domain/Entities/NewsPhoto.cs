using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities;

public sealed class NewsPhoto : AuditableEntity<IdOf<NewsPhoto>>
{
    public long ExtId { get; }
    public Uri Uri { get; }

    public IdOf<News> NewsId { get; }
    public News? News { get; }

    public NewsPrimaryPhoto? PrimaryPhoto { get; }
}