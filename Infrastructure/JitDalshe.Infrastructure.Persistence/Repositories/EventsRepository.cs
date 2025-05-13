using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
internal sealed class EventsRepository : IEventsRepository
{
    private readonly PostgresqlDbContext _dbContext;

    public EventsRepository(PostgresqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Event[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<Event, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default)
    {
        var query = _dbContext.Events
            .Include(x => x.Image)
            .AsQueryable();

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            query = query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        if (orderByExpression is not null)
        {
            query = sortingOrder switch
            {
                SortingOrder.Ascending => query.OrderBy(orderByExpression),
                SortingOrder.Descending => query.OrderByDescending(orderByExpression),
                _ => query
            };
        }

        return query.ToArrayAsync(ct);
    }

    public Task<Maybe<Event>> FindByIdAsync(IdOf<Event> id, CancellationToken ct = default)
        => _dbContext.Events
            .Include(x => x.Image)
            .TryFirstAsync(x => x.Id == id, ct);

    public async Task AddAsync(Event @event, CancellationToken ct = default)
    {
        _dbContext.Events.Add(@event);
        _dbContext.EventImages.Add(@event.Image!);
        await _dbContext.SaveChangesAsync(ct);
    }
}