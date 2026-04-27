using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IVolunteerRequestsRepository
{
    Task<VolunteerRequest[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<VolunteerRequest, bool>>? filteringExpression = null,
        Expression<Func<VolunteerRequest, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default);

    Task<Maybe<VolunteerRequest>> FindByIdAsync(IdOf<VolunteerRequest> id, CancellationToken ct = default);
    Task AddAsync(VolunteerRequest request, CancellationToken ct = default);
    Task EditAsync(VolunteerRequest request, CancellationToken ct = default);
    Task<int> CountAsync(Expression<Func<VolunteerRequest, bool>>? filteringExpression = null, CancellationToken ct = default);
}