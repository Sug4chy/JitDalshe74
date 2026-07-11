using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.AdminUsers.DeleteAdminUser;

public interface IDeleteAdminUserUseCase
{
    Task<UnitResult<Error>> DeleteAsync(IdOf<AdminUser> id, CancellationToken ct = default);
}