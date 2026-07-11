using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.Entities.Users;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IAdminUsersRepository
{
    Task<Maybe<AdminUser>> FindByIdAsync(IdOf<AdminUser> id,  CancellationToken ct = default);
    Task<Maybe<AdminUser>> FindByEmailAsync(string email, CancellationToken ct = default);
    Task AddAsync(AdminUser user, CancellationToken ct = default);
    Task EditAsync(AdminUser user, CancellationToken ct = default);
    
    Task<AdminUser[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<AdminUser, bool>>? filteringExpression = null,
        Expression<Func<AdminUser, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default);
    
    Task<int> CountAsync(Expression<Func<AdminUser, bool>>? filteringExpression = null, CancellationToken ct = default);
    
    Task RemoveAsync(AdminUser user, CancellationToken ct = default);
}