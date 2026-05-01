using System.Linq.Expressions;
using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Events;

namespace JitDalshe.Application.Admin.UseCases.Events.ListEvents;

[UseCase]
public sealed class ListEventsUseCase : IListEventsUseCase
{
    private readonly IEventsRepository _events;

    public ListEventsUseCase(IEventsRepository events)
    {
        _events = events;
    }

    public async Task<Result<PagedResult<EventDto>, Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        try
        {
            Expression<Func<Event, bool>>? filteringExpression = null; // На случай, если нужен будет фильтр

            var totalCount = await _events.CountAsync(filteringExpression, ct);
            
            var events = await _events.FindAllAsync(
                pageNumber : pageNumber,
                pageSize: pageSize,
                filteringExpression: filteringExpression,
                orderByExpression: x => x.Date,
                sortingOrder: SortingOrder.Descending,
                ct: ct);

            var dtos = events.Select(x => x.ToDto()).ToArray();
            
            return Result.Success<PagedResult<EventDto>, Error>(
                new PagedResult<EventDto>(
                    Items: dtos, 
                    TotalCount: totalCount, 
                    PageNumber: pageNumber, 
                    PageSize: pageSize)
            );
        }
        catch (Exception e)
        {
            return Result.Failure<PagedResult<EventDto>, Error>(Error.Of(e.Message));
        }
    }
}