namespace JitDalshe.Ui.Admin.Models;

public sealed record Review(
    Guid Id,
    string ReviewerName,
    int ReviewerAge,
    ReviewerStatus ReviewerStatus,
    string Text,
    ReviewStatus Status,
    DateOnly Date
)
{
    private string ReviewerAgesLiteral => (ReviewerAge % 10) switch
    {
        1 => "год",
        >= 2 and <= 4 => "года",
        _ => "лет"
    };

    public string ReviewerAgeString => $"{ReviewerAge} {ReviewerAgesLiteral}";

    public string ReviewerStatusString => ReviewerStatus switch
    {
        ReviewerStatus.Patient => "Пациент",
        ReviewerStatus.PatientRelative => "Родственник пациента",
        ReviewerStatus.Other => "Другое",
        _ => throw new ArgumentOutOfRangeException()
    };
}

public enum ReviewerStatus
{
    Patient,
    PatientRelative,
    Other
}
    
public enum ReviewStatus
{
    New,
    InProgress,
    Published,
    NotPublished
}