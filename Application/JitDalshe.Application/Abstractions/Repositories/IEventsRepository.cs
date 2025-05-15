using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Enums;
using JitDalshe.Domain.Entities.Events;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Abstractions.Repositories;

public interface IEventsRepository
{
    Task<Event[]> FindAllAsync<TOrderKey>(
        int? pageNumber = null,
        int? pageSize = null,
        Expression<Func<Event, TOrderKey>>? orderByExpression = null,
        SortingOrder sortingOrder = SortingOrder.Ascending,
        CancellationToken ct = default);

    Task<Maybe<Event>> FindByIdAsync(IdOf<Event> id, CancellationToken ct = default);
    Task AddAsync(Event @event, CancellationToken ct = default);
    Task EditAsync(Event @event, CancellationToken ct = default);
    Task ReplaceEventImageAsync(Event @event, EventImage newImage, CancellationToken ct = default);
    Task RemoveAsync(Event @event, CancellationToken ct = default);
}