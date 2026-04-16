using JitDalshe.Ui.Admin.Models;
using Refit;


namespace JitDalshe.Ui.Admin.Api.Consultations;

public interface IConsultationsApiClient
{
    [Get("")]
    Task<ApiResponse<ConsultationRequest[]>> ListAsync();

    [Patch("/{id}/status")]
    Task<ApiResponse<IApiResponse>> ChangeStatusAsync(Guid id); 

}