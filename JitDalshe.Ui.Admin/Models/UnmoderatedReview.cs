namespace JitDalshe.Ui.Admin.Models;

public sealed record UnmoderatedReview(
    Guid Id,
    string ReviewerName,
    int ReviewerAge,
    ReviewerStatus ReviewerStatus,
    string Text,
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