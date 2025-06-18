namespace JitDalshe.Ui.Admin.Models;

public sealed record News(
    Guid Id,
    string Text,
    DateOnly PublishDate,
    NewsImage[] Images,
    string PostUrl)
{
    public string Title => Text.Split('.')[0];

    public string TextWithoutTitle => string.Join('.', Text.Split('.')[1..]);
}