using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;
using JitDalshe.Infrastructure.Persistence.Attributes;
using JitDalshe.Infrastructure.Persistence.Context;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace JitDalshe.Infrastructure.Persistence.Repositories;

[Repository]
internal sealed class EventsRepository :  IEventsRepository
{
    private readonly PostgresqlDbContext _dbContext;

    public EventsRepository(PostgresqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<Event[]> FindAllAsync(CancellationToken ct = default)
        => _dbContext.Events
            .Include(x => x.Image)
            .ToArrayAsync(ct);

    public Task<Maybe<Event>> FindByIdAsync(IdOf<Event> id, CancellationToken ct = default)
        => _dbContext.Events
            .Include(x => x.Image)
            .TryFirstAsync(x => x.Id == id, ct);
}