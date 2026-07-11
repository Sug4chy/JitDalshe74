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
    private readonly IErrorHandlers _errorHandlers;

    public SupportGroupService(
        Runner runner, 
        ISupportGroupsApiClient supportGroupsApi, 
        IToastService toastService, 
        IErrorHandlers errorHandlers)
    {
        _runner = runner;
        _supportGroupsApi = supportGroupsApi;
        _errorHandlers = errorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PagedResult<SupportGroupRequest>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default) 
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _supportGroupsApi.ListAsync(pageNumber, pageSize, status, startDate, endDate, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: null);

    public Task<bool> ChangeStatusAsync(Guid id, RequestStatus status, CancellationToken ct = default) 
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new ChangeSupportGroupRequestStatusRequest(status);
            var response = await _supportGroupsApi.ChangeStatusAsync(id, request, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: false);
    
    public Task<bool> UpdateCommentAsync(Guid id, string? comment, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new UpdateSupportGroupRequestCommentRequest(comment);
            var response = await _supportGroupsApi.UpdateCommentAsync(id, request, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: false);
}