using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Volunteers;
using JitDalshe.Ui.Admin.Api.Volunteers.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Models.Common;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.VolunteerService;

public sealed class VolunteerService : IVolunteerService
{
    private readonly Runner _runner;
    private readonly IVolunteersApiClient _volunteersApi;
    private readonly IErrorHandlers _errorHandlers;

    public VolunteerService(
        Runner runner, 
        IVolunteersApiClient volunteersApi, 
        IToastService toastService, 
        IErrorHandlers errorHandlers)
    {
        _runner = runner;
        _volunteersApi = volunteersApi;
        _errorHandlers = errorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PagedResult<VolunteerRequest>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default) 
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _volunteersApi.ListAsync(pageNumber, pageSize, status, startDate, endDate, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: null);

    public Task<bool> ChangeStatusAsync(Guid id, RequestStatus status, CancellationToken ct = default) 
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new ChangeVolunteerRequestStatusRequest(status);
            var response = await _volunteersApi.ChangeStatusAsync(id, request, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: false);
    
    public Task<bool> UpdateCommentAsync(Guid id, string? comment, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new UpdateVolunteerRequestCommentRequest(comment);
            var response = await _volunteersApi.UpdateCommentAsync(id, request, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: false);
}