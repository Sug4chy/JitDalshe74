namespace JitDalshe.Ui.Admin.Models;

public sealed record News(
    Guid Id,
    string Text,
    DateOnly PublishDate,
    NewsPhoto[] Photos)
{
    public string Title => Text.Split('.')[0];

    public string TextWithoutTitle => string.Join('.', Text.Split('.')[1..]);
}