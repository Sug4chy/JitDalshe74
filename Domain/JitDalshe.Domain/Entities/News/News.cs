using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.News;

public sealed class News : AuditableEntity<IdOf<News>>
{
    public long ExtId { get; init; }
    public string Text { get; set; }
    public DateOnly PublicationDate { get; init; }
    public string PostUrl { get; init; }
    public bool IsDisplaying { get; set; }

    public ICollection<NewsImage> Images { get; init; } = [];

    public NewsPrimaryImage? PrimaryImage { get; set; }

    private News(
        IdOf<News> id,
        long extId,
        string text,
        DateOnly publicationDate,
        string postUrl,
        bool isDisplaying,
        ICollection<NewsImage> images)
    {
        Id = id;
        ExtId = extId;
        Text = text;
        PublicationDate = publicationDate;
        PostUrl = postUrl;
        IsDisplaying = isDisplaying;
        Images = images;
    }

    public static News Create(
        IdOf<News> id,
        long extId, 
        string text, 
        DateOnly publicationDate, 
        string postUrl,
        bool isDisplaying, 
        ICollection<NewsImage> images)
        => new(id, extId, text, publicationDate, postUrl, isDisplaying, images);

    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private News()
    {
    }
#pragma warning restore CS8618
}