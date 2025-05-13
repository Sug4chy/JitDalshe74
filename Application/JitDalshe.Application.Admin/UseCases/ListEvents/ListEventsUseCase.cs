using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.Extensions;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Enums;
using JitDalshe.Application.Errors;

namespace JitDalshe.Application.Admin.UseCases.ListEvents;

[UseCase]
public sealed class ListEventsUseCase : IListEventsUseCase
{
    private readonly IEventsRepository _events;

    public ListEventsUseCase(IEventsRepository events)
    {
        _events = events;
    }

    public async Task<Result<EventDto[], Error>> ListAsync(CancellationToken ct = default)
    {
        try
        {
            var events = await _events.FindAllAsync(
                orderByExpression: x => x.Date,
                sortingOrder: SortingOrder.Descending,
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