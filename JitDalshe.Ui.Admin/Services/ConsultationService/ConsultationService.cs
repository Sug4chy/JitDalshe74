using System.Net;
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
    private readonly CommonErrorHandlers _commonErrorHandlers;

    public ConsultationService(
        Runner runner, 
        IConsultationsApiClient consultationsApi, 
        IToastService toastService, 
        CommonErrorHandlers commonErrorHandlers)
    {
        _runner = runner;
        _consultationsApi = consultationsApi;
        _commonErrorHandlers = commonErrorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PagedResult<ConsultationRequest>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default
        ) => _runner.RunCatchingAsync(async () =>
    {
        var response = await _consultationsApi.ListAsync(pageNumber, pageSize, status, startDate, endDate, ct);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return response.Content;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, defaultValue: null);

    public Task<bool> ChangeStatusAsync(Guid id, RequestStatus status, CancellationToken ct = default) 
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new ChangeConsultationRequestStatusRequest(status);
            var response = await _consultationsApi.ChangeStatusAsync(id, request, ct);

            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.NotFound:
                    _commonErrorHandlers.HandleNotFound(response.Error!);
                    return false;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }, defaultValue: false);
    
    public Task<bool> UpdateCommentAsync(Guid id, string? comment, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new UpdateConsultationRequestCommentRequest(comment);
            var response = await _consultationsApi.UpdateCommentAsync(id, request, ct);
            
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.NotFound:
                    _commonErrorHandlers.HandleNotFound(response.Error!);
                    return false;
                case HttpStatusCode.InternalServerError:
                    _commonErrorHandlers.HandleInternalServerError(response.Error!);
                    return false;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
        }, defaultValue: false);
}