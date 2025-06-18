using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Dto;
using JitDalshe.Application.Site.Extensions;

namespace JitDalshe.Application.Site.UseCases.ListEvents;

[UseCase]
internal sealed class ListEventsUseCase : IListEventsUseCase
{
    private readonly IEventsRepository _events;

    public ListEventsUseCase(IEventsRepository events)
    {
        _events = events;
    }

    public async Task<Result<EventDto[], Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        try
        {
            var events = await _events.FindAllAsync(
                orderByExpression: x => x.Date,
                sortingOrder: SortingOrder.Ascending,
                filteringExpression: x => x.Date > DateOnly.FromDateTime(DateTime.Now),
                pageNumber: pageNumber,
                pageSize: pageSize,
                ct: ct);

            return Result.Success<EventDto[], Error>(
                events
                    .Select(x => x.ToDto())
                    .ToArray()
            );
        }
        catch (Exception e)
        {
            return Result.Failure<EventDto[], Error>(Error.Of(e.Message));
        }
    }
}