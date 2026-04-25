using JitDalshe.Ui.Admin.Api.Consultations.Requests;
using JitDalshe.Ui.Admin.Models;

namespace JitDalshe.Ui.Admin.Services.ConsultationService;

public interface IConsultationService
{
    Task<PagedResult<ConsultationRequest>?> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
    Task<bool> ChangeStatusAsync(Guid id, ConsultationRequestStatus status, CancellationToken ct = default);
    Task<bool> UpdateCommentAsync(Guid id, string? comment, CancellationToken ct = default);
}