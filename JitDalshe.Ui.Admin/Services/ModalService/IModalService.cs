namespace JitDalshe.Ui.Admin.Services.ModalService;

public interface IModalService
{
    Task ShowModalAsync(string id, CancellationToken ct = default);
    Task HideModalAsync(string id, CancellationToken ct = default);
}