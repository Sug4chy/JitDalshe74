using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities;

public sealed class News : AuditableEntity<IdOf<News>>
{
    public long ExtId { get; }
    public string Text { get; set; }
    public DateOnly PublicationDate { get; }

    public ICollection<NewsPhoto> Photos { get; }

    public NewsPrimaryPhoto? PrimaryPhoto { get; set; }
}