using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Users;

namespace JitDalshe.Application.Admin.UseCases.AdminUsers.ListAdminUsers;

[UseCase]
public class ListAdminUsersUseCase(IAdminUsersRepository repository) : IListAdminUsersUseCase
{
    public async Task<Result<PagedResult<AdminUserDto>, Error>> ListAsync(int pageNumber, int pageSize, bool? isActive = null, CancellationToken ct = default)
    {
        try
        {
            Expression<Func<AdminUser, bool>>? filteringExpression = isActive.HasValue
                ? x => x.IsActive == isActive.Value
                : null;

            var users = await repository.FindAllAsync(
                pageNumber: pageNumber,
                pageSize: pageSize,
                filteringExpression: filteringExpression,
                orderByExpression: x => x.CreatedAt,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            var totalCount = await repository.CountAsync(filteringExpression, ct);

            var dtos = users.Select(x => x.ToDto()).ToArray();

            var pagedResult = new PagedResult<AdminUserDto>(
                Items: dtos,
                TotalCount: totalCount,
                PageNumber: pageNumber,
                PageSize: pageSize
            );

            return Result.Success<PagedResult<AdminUserDto>, Error>(pagedResult);
        }
        catch (Exception e)
        {
            return Result.Failure<PagedResult<AdminUserDto>, Error>(Error.Of(e.Message));
        }
    }
}