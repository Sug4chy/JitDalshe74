using System.Net;
using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.AdminUsers;
using JitDalshe.Ui.Admin.Api.AdminUsers.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Models;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;

namespace JitDalshe.Ui.Admin.Services.AdminUserService;

public sealed class AdminUserService : IAdminUserService
{
    private readonly Runner _runner;
    private readonly IAdminUsersApiClient _apiClient;
    private readonly IErrorHandlers _errorHandlers;

    public AdminUserService(
        Runner runner,
        IAdminUsersApiClient apiClient,
        IToastService toastService,
        IErrorHandlers errorHandlers)
    {
        _runner = runner;
        _apiClient = apiClient;
        _errorHandlers = errorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }

    public Task<PagedResult<AdminUser>?> ListAsync(int pageNumber, int pageSize, bool? isActive = null, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _apiClient.ListAdminUsersAsync(pageNumber, pageSize, isActive, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: null);

    public Task<bool> CreateAsync(string email, string password, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new CreateAdminRequest(email, password);
            var response = await _apiClient.CreateAdminUserAsync(request, ct);
            return response.Handle(_errorHandlers, HttpStatusCode.Created);
        }, defaultValue: false);

    public Task<bool> ToggleStatusAsync(Guid id, bool isActive, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var request = new ChangeAdminStatusRequest(isActive);
            var response = await _apiClient.ChangeAdminUserActiveStatusAsync(id, request, ct);
            return response.Handle(_errorHandlers);
        }, defaultValue: false);
    
    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _apiClient.DeleteAdminUserAsync(id, ct); // Ой, стоп, мы еще не добавили метод в клиент
            return response.Handle(_errorHandlers, HttpStatusCode.NoContent);
        }, defaultValue: false);
}