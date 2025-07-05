using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Reviews;

public sealed class Review : AuditableEntity<IdOf<Review>>
{
    public string ReviewerName { get; init; }
    public int ReviewerAge { get; init; }
    public ReviewerStatus ReviewerStatus { get; init; }
    public string Text { get; init; }
    public bool IsModerated { get; set; }

    private Review(string reviewerName, int reviewerAge, ReviewerStatus reviewerStatus, string text, bool isModerated)
    {
        Id = IdOf<Review>.New();
        ReviewerName = reviewerName;
        ReviewerAge = reviewerAge;
        ReviewerStatus = reviewerStatus;
        Text = text;
        IsModerated = isModerated;
    }

    public static Review Create(
        string reviewerName,
        int reviewerAge,
        ReviewerStatus reviewerStatus,
        string text,
        bool isModerated)
        => new(reviewerName, reviewerAge, reviewerStatus, text, isModerated);

    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private Review()
    {
    }
#pragma warning restore CS8618
}