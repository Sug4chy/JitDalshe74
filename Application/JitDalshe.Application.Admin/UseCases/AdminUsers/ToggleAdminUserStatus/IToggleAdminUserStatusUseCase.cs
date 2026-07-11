using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.AdminUsers.ToggleAdminUserStatus;

public interface IToggleAdminUserStatusUseCase
{
    Task<UnitResult<Error>> ChangeStatusAsync(IdOf<AdminUser> id, bool isActive, CancellationToken ct = default);
}