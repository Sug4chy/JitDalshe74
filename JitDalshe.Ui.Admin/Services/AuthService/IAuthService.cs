using JitDalshe.Ui.Admin.Api.Auth.Requests;

namespace JitDalshe.Ui.Admin.Services.AuthService;

public interface IAuthService
{
    Task<string?> GetTokenAsync();
    Task<string?> GetCurrentUserRoleAsync();
    Task<bool> LoginAsync(LoginRequest request);
    Task LogoutAsync();
}