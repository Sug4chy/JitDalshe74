using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;


[Repository]
public class VolunteerRequestsRepository : IVolunteerRequestsRepository
{
    private PostgresqlDbContext _dbContext;

    public VolunteerRequestsRepository(PostgresqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<VolunteerRequest[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null, 
        int? pageSize = null, 
        Expression<Func<VolunteerRequest, bool>>? filteringExpression = null,
        Expression<Func<VolunteerRequest, TOrderKey>>? orderByExpression = null, 
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default)
    {
        IQueryable<VolunteerRequest> query = _dbContext.VolunteerRequests.AsQueryable();

        if (filteringExpression is not null)
            query = query.Where(filteringExpression);

        if (orderByExpression is not null)
            query = sortingOrder == SortingOrder.Ascending
                ? query.OrderBy(orderByExpression)
                : query.OrderByDescending(orderByExpression);

        if (pageNumber.HasValue && pageSize.HasValue)
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);

        return await query.AsNoTracking().ToArrayAsync(ct);
    }

    public async Task<Maybe<VolunteerRequest>> FindByIdAsync(IdOf<VolunteerRequest> id, CancellationToken ct = default)
    {
        var request = await _dbContext.VolunteerRequests.FirstOrDefaultAsync(x => x.Id == id, ct);
        return Maybe<VolunteerRequest>.From(request);
    }

    public async Task AddAsync(VolunteerRequest request, CancellationToken ct = default)
    {
        _dbContext.VolunteerRequests.Add(request);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task EditAsync(VolunteerRequest request, CancellationToken ct = default)
    {
        _dbContext.VolunteerRequests.Update(request);
        await _dbContext.SaveChangesAsync(ct);
    }

    public async Task<int> CountAsync(Expression<Func<VolunteerRequest, bool>>? filteringExpression = null, CancellationToken ct = default)
    {
        var query = _dbContext.Set<VolunteerRequest>().AsQueryable();

        if (filteringExpression is not null)
        {
            query = query.Where(filteringExpression);
        }
        
        return await query.CountAsync(ct);
    }
}