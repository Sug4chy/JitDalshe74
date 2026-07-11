using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.Auth.Login;

public interface ILoginUseCase
{
    Task<Result<string, Error>> LoginAsync(string email, string password, CancellationToken ct = default);
}