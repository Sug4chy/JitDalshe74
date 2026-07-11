using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.AdminUsers.CreateAdminUser;

public interface ICreateAdminUserUseCase
{
    Task<UnitResult<Error>> CreateAsync(string email, string password, CancellationToken ct = default);
}