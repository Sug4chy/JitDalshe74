namespace JitDalshe.Application.Admin.UseCases.Auth.Login;

public interface ILoginUseCase
{
    Task<LoginResult> LoginAsync(string email, string password, CancellationToken ct = default);
}