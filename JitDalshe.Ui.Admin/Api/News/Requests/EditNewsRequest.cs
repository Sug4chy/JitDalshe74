namespace JitDalshe.Ui.Admin.Api.News.Requests;

public sealed class EditNewsRequest
{ 
    public required string Text { get; init; }
    public Guid? PrimaryPhotoId { get; init; } = null;
}