using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities;

public sealed class News : AuditableEntity<IdOf<News>>
{
    public long ExtId { get; init; }
    public string Text { get; set; }
    public DateOnly PublicationDate { get; init; }

    public ICollection<NewsPhoto> Photos { get; init; }

    public NewsPrimaryPhoto? PrimaryPhoto { get; set; }

    public News()
    {
        Id = IdOf<News>.New();
        CreatedAt = DateTime.UtcNow;
    }
}