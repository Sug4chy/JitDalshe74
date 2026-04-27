using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface ISupportGroupRequestsRepository
{
    Task<SupportGroupRequest[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<SupportGroupRequest, bool>>? filteringExpression = null,
        Expression<Func<SupportGroupRequest, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default);
    
    Task<Maybe<SupportGroupRequest>> FindByIdAsync(IdOf<SupportGroupRequest> id, CancellationToken ct = default);
    
    Task AddAsync(SupportGroupRequest request, CancellationToken ct = default);
    
    Task EditAsync(SupportGroupRequest request, CancellationToken ct = default);
    
    Task<int> CountAsync(Expression<Func<SupportGroupRequest, bool>>? filteringExpression = null, CancellationToken ct = default);
}