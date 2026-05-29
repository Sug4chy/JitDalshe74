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
    public ReviewStatus Status { get; private set; }

    private Review(string reviewerName, int reviewerAge, ReviewerStatus reviewerStatus, string text, ReviewStatus status)
    {
        Id = IdOf<Review>.New();
        ReviewerName = reviewerName;
        ReviewerAge = reviewerAge;
        ReviewerStatus = reviewerStatus;
        Text = text;
        Status = status;
    }

    public static Review Create(
        string reviewerName,
        int reviewerAge,
        ReviewerStatus reviewerStatus,
        string text,
        ReviewStatus status = ReviewStatus.New)
        => new(reviewerName, reviewerAge, reviewerStatus, text, status);

    public void ChangeStatus(ReviewStatus newStatus)
    {
        Status = newStatus;
    }
    
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