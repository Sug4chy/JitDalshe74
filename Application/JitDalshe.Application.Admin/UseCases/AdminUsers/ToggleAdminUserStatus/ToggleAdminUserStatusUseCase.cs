using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.AdminUsers.ToggleAdminUserStatus;

[UseCase]
internal sealed class ToggleAdminUserStatusUseCase : IToggleAdminUserStatusUseCase
{
    private readonly IAdminUsersRepository _repository;

    public ToggleAdminUserStatusUseCase(IAdminUsersRepository repository)
    {
        _repository = repository;
    }

    public async Task<UnitResult<Error>> ChangeStatusAsync(IdOf<AdminUser> id, bool isActive, CancellationToken ct = default)
    {
        try
        {
            var maybeUser = await _repository.FindByIdAsync(id, ct);
            if (maybeUser.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Администратор не найден", ErrorGroup.NotFound));
            }

            var user = maybeUser.Value;
            if (isActive)
            {
                user.Activate();
            }
            else
            {
                user.Deactivate();
            }

            await _repository.EditAsync(user, ct);
            return UnitResult.Success<Error>();
        }
        catch (InvalidOperationException ex)
        {
            return UnitResult.Failure(Error.Of(ex.Message));
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}