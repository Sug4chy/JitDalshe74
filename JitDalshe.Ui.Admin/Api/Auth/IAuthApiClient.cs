using JitDalshe.Ui.Admin.Api.Auth.Requests;
using Refit;

namespace JitDalshe.Ui.Admin.Api.Auth;

public interface IAuthApiClient
{
    [Post("/login")]
    Task<IApiResponse> LoginAsync([Body] LoginRequest request);
    
    [Post("/logout")]
    Task<IApiResponse> LogoutAsync();
}