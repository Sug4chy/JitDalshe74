namespace JitDalshe.Ui.Admin.Models;

public sealed record News(
    Guid Id,
    DateOnly PublishDate,
    NewsPhoto[] Photos)
{
    public string Text { get; set; } = string.Empty;

    public string Title => Text.Split('.')[0];

    public string TextWithoutTitle => string.Join('.', Text.Split('.')[1..]);
}