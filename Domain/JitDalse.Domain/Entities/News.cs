using JitDalse.Domain.Abstractions;
using JitDalse.Domain.ValueObjects;

namespace JitDalse.Domain.Entities;

public sealed class News : AuditableEntity<IdOf<News>>
{
    public long ExtId { get; }
    public string Text { get; }
    public DateOnly PublicationDate { get; }

    public ICollection<NewsPhoto> Photos { get; }

    public NewsPrimaryPhoto? PrimaryPhoto { get; }
}