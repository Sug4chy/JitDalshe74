using CSharpFunctionalExtensions;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;

namespace JitDalshe.Application.Admin.UseCases.AdminUsers.ListAdminUsers;

public interface IListAdminUsersUseCase
{
    Task<Result<PagedResult<AdminUserDto>, Error>> ListAsync(
        int pageNumber,
        int pageSize,
        bool? isActive = null,
        CancellationToken ct = default
    );
}