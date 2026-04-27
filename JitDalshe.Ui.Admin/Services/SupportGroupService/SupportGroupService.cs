using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.SupportGroups;
using JitDalshe.Ui.Admin.Api.SupportGroups.Requests;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Models.Common;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;
using JitDalshe.Ui.Admin.Extensions;
namespace JitDalshe.Ui.Admin.Services.SupportGroupService;

public sealed class SupportGroupService : ISupportGroupService
{
        private readonly Runner _runner;
    private readonly ISupportGroupsApiClient _supportGroupsApi;
    private readonly CommonErrorHandlers _commonErrorHandlers;

    public SupportGroupService(
        Runner runner, 
        ISupportGroupsApiClient supportGroupsApi, 
        IToastService toastService, 
        CommonErrorHandlers commonErrorHandlers)
    {
        _runner = runner;
        _supportGroupsApi = supportGroupsApi;
        _commonErrorHandlers = commonErrorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PagedResult<SupportGroupRequest>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default
        ) => _runner.RunCatchingAsync(async () =>
    {
        var response = await _supportGroupsApi.ListAsync(pageNumber, pageSize, status, startDate, endDate, ct);

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
            var request = new ChangeSupportGroupRequestStatusRequest(status);
            var response = await _supportGroupsApi.ChangeStatusAsync(id, request, ct);

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
            var request = new UpdateSupportGroupRequestCommentRequest(comment);
            var response = await _supportGroupsApi.UpdateCommentAsync(id, request, ct);
            
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