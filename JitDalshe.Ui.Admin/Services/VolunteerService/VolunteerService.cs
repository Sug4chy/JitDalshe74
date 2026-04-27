using System.Net;
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
    private readonly CommonErrorHandlers _commonErrorHandlers;

    public VolunteerService(
        Runner runner, 
        IVolunteersApiClient volunteersApi, 
        IToastService toastService, 
        CommonErrorHandlers commonErrorHandlers)
    {
        _runner = runner;
        _volunteersApi = volunteersApi;
        _commonErrorHandlers = commonErrorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PagedResult<VolunteerRequest>?> ListAsync(
        int pageNumber, 
        int pageSize, 
        RequestStatus? status = null,
        DateOnly? startDate = null,
        DateOnly? endDate = null,
        CancellationToken ct = default
        ) => _runner.RunCatchingAsync(async () =>
    {
        var response = await _volunteersApi.ListAsync(pageNumber, pageSize, status, startDate, endDate, ct);

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
            var request = new ChangeVolunteerRequestStatusRequest(status);
            var response = await _volunteersApi.ChangeStatusAsync(id, request, ct);

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
            var request = new UpdateVolunteerRequestCommentRequest(comment);
            var response = await _volunteersApi.UpdateCommentAsync(id, request, ct);
            
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