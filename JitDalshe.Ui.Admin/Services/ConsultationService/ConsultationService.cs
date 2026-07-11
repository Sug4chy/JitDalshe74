using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Consultations;
using JitDalshe.Ui.Admin.Api.Consultations.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Models.Common;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.ConsultationService;

public sealed class ConsultationService : IConsultationService
{
    private readonly Runner _runner;
    private readonly IConsultationsApiClient _consultationsApi;
    private readonly IErrorHandlers _errorHandlers;

    public ConsultationService(
        Runner runner, 
        IConsultationsApiClient consultationsApi, 
        IToastService toastService, 
        IErrorHandlers errorHandlers)
    {
        _runner = runner;
        _consultationsApi = consultationsApi;
        _errorHandlers = errorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PagedResult<ConsultationRequest>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default) 
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _consultationsApi.ListAsync(pageNumber, pageSize, status, startDate, endDate, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: null);

    public Task<bool> ChangeStatusAsync(Guid id, RequestStatus status, CancellationToken ct = default) 
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new ChangeConsultationRequestStatusRequest(status);
            var response = await _consultationsApi.ChangeStatusAsync(id, request, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: false);
    
    public Task<bool> UpdateCommentAsync(Guid id, string? comment, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new UpdateConsultationRequestCommentRequest(comment);
            var response = await _consultationsApi.UpdateCommentAsync(id, request, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: false);
}