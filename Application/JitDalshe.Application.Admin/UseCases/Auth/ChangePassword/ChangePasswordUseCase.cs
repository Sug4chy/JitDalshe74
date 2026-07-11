using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Abstractions.Security;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Auth.ChangePassword;

[UseCase]
internal sealed class ChangePasswordUseCase(IAdminUsersRepository repository, IPasswordHasher passwordHasher) 
    : IChangePasswordUseCase
{
    public async Task<UnitResult<Error>> ChangeAsync(IdOf<AdminUser> userId, string currentPassword, string newPassword, CancellationToken ct = default)
    {
        try
        {
            var maybeUser = await repository.FindByIdAsync(userId, ct);
            if (maybeUser.HasNoValue || !maybeUser.Value.IsActive)
            {
                return UnitResult.Failure(Error.Of("Пользователь не найден", ErrorGroup.NotFound));
            }

            var user = maybeUser.Value;
            if (!passwordHasher.Verify(currentPassword, user.PasswordHash))
            {
                return UnitResult.Failure(Error.Of("Текущий пароль указан неверно", ErrorGroup.Unauthorized));
            }

            var newPasswordHash = passwordHasher.Hash(newPassword);
            user.ChangePassword(newPasswordHash);

            await repository.EditAsync(user, ct);
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}