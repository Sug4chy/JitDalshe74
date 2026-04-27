using JitDalshe.Ui.Admin.Models;
using Refit;
using JitDalshe.Ui.Admin.Api.Consultations.Requests;
using JitDalshe.Ui.Admin.Models.Common;


namespace JitDalshe.Ui.Admin.Api.Consultations;

public interface IConsultationsApiClient
{
    [Get("")]
    Task<ApiResponse<PagedResult<ConsultationRequest>>> ListAsync(
        [Query] int pageNumber, 
        [Query] int pageSize, 
        [Query] RequestStatus? status,
        [Query] DateOnly? startDate,
        [Query] DateOnly? endDate,
        CancellationToken ct = default);

    [Patch("/{id}/status")]
    Task<ApiResponse<IApiResponse>> ChangeStatusAsync(Guid id, ChangeConsultationRequestStatusRequest request, CancellationToken ct = default); 

    [Patch("/{id}/comment")]
    Task<ApiResponse<IApiResponse>> UpdateCommentAsync(Guid id, UpdateConsultationRequestCommentRequest request, CancellationToken ct = default);
}