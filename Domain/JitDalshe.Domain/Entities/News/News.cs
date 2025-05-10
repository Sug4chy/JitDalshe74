using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.News;

public sealed class News : AuditableEntity<IdOf<News>>
{
    public long ExtId { get; init; }
    public string Text { get; set; }
    public DateOnly PublicationDate { get; init; }

    public ICollection<NewsImage> Images { get; init; }

    public NewsPrimaryImage? PrimaryImage { get; set; }

    public News()
    {
        Id = IdOf<News>.New();
    }
}