using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.ConsultationService;

public interface IConsultationService
{
    Task<ConsultationRequest[]> ListAsync();
    Task<bool> ChangeStatusAsync(Guid id, ConsultationRequestStatus status, CancellationToken ct = default);
}