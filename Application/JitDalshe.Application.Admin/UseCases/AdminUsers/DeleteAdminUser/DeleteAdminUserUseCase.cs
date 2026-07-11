using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.AdminUsers.DeleteAdminUser;

[UseCase]
internal sealed class DeleteAdminUserUseCase(IAdminUsersRepository repository) : IDeleteAdminUserUseCase
{
    public async Task<UnitResult<Error>> DeleteAsync(IdOf<AdminUser> id, CancellationToken ct = default)
    {
        try
        {
            var maybeUser = await repository.FindByIdAsync(id, ct);
            if (maybeUser.HasNoValue)
                return UnitResult.Failure(Error.Of("Администратор не найден", ErrorGroup.NotFound));

            var user = maybeUser.Value;
            
            if (user.Role == UserRole.SuperAdmin)
                return UnitResult.Failure(Error.Of("Нельзя удалить администратора с правами суперадмина"));

            await repository.RemoveAsync(user, ct);
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}