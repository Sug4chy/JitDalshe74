using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
internal sealed class ConsultationRequestsRepository : IConsultationRequestsRepository
{
    private readonly PostgresqlDbContext _db;

    public ConsultationRequestsRepository(PostgresqlDbContext db)
    {
        _db = db;
    }
    
    public async Task<ConsultationRequest[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<ConsultationRequest, bool>>? filteringExpression = null,
        Expression<Func<ConsultationRequest, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default)
    {
        IQueryable<ConsultationRequest> query = _db.ConsultationRequests.AsQueryable();

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

    public async Task<Maybe<ConsultationRequest>> FindByIdAsync(IdOf<ConsultationRequest> id, CancellationToken ct = default)
    {
        var request = await _db.ConsultationRequests.FirstOrDefaultAsync(x => x.Id == id, ct);
        return Maybe<ConsultationRequest>.From(request);
    }

    public async Task AddAsync(ConsultationRequest request, CancellationToken ct = default)
    {
        _db.ConsultationRequests.Add(request);
        await _db.SaveChangesAsync(ct);
    }
    
    public async Task EditAsync(ConsultationRequest request, CancellationToken ct = default)
    {
        _db.ConsultationRequests.Update(request);
        await _db.SaveChangesAsync(ct);
    }
}