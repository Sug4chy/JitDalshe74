using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Auth.ChangePassword;

public interface IChangePasswordUseCase
{
    Task<UnitResult<Error>> ChangeAsync(IdOf<AdminUser> userId, string currentPassword, string newPassword, CancellationToken ct = default);
}