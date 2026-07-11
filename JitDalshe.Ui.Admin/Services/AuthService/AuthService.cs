using Blazored.Toast.Services;
using JitDalshe.Ui.Admin.Api.Auth;
using JitDalshe.Ui.Admin.Api.Auth.Requests;
using JitDalshe.Ui.Admin.Extensions;
using JitDalshe.Ui.Admin.Services.ErrorHandlers;
using JitDalshe.Ui.Admin.Services.Shared;
using Microsoft.JSInterop;

namespace JitDalshe.Ui.Admin.Services.AuthService;

public sealed class AuthService : IAuthService
{
    private readonly IAuthApiClient _authApi;
    private readonly IJSRuntime _js;
    private readonly Runner _runner;
    private readonly IErrorHandlers _errorHandlers;

    public AuthService(
        IAuthApiClient authApi,
        IJSRuntime js,
        Runner runner,
        IErrorHandlers errorHandlers,
        IToastService toastService)
    {
        _authApi = authApi;
        _js = js;
        _runner = runner;
        _errorHandlers = errorHandlers;
        _runner.ConfigureErrorCallback(toastService.ShowPermanentError);
    }
    
    public Task<string?> GetTokenAsync()
        => _runner.RunCatchingAsync(async () => await GetCookieAsync("userRole"));

    public async Task<string?> GetCurrentUserRoleAsync()
    {
        return await GetCookieAsync("userRole");
    }

    public Task<bool> LoginAsync(LoginRequest request)
        => _runner.RunCatchingAsync(async () =>
        {
            var response = await _authApi.LoginAsync(request);
            
            return response.Handle(_errorHandlers);
        }, defaultValue: false);

    public Task LogoutAsync()
        => _runner.RunCatchingAsync(async () => 
        {
            var response = await _authApi.LogoutAsync();
            response.Handle(_errorHandlers);
        });
    
    private async Task<string?> GetCookieAsync(string name)
    {
        try
        {
            var cookieString = await _js.InvokeAsync<string>("eval", "document.cookie");
            if (string.IsNullOrWhiteSpace(cookieString)) return null;

            var cookies = cookieString.Split(';')
                .Select(c => c.Trim().Split('='))
                .Where(c => c.Length > 0 && !string.IsNullOrWhiteSpace(c[0]))
                .ToDictionary(c => c[0], c => c.Length > 1 ? Uri.UnescapeDataString(c[1]) : "");

            return cookies.GetValueOrDefault(name);
        }
        catch
        {
            return null;
        }
    }
}