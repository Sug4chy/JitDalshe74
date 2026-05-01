using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Dto;
using JitDalshe.Application.Site.Dto.Events;

namespace JitDalshe.Application.Site.UseCases.Events.ListEvents;

public interface IListEventsUseCase
{
    Task<Result<EventPreviewDto[], Error>> ListAsync(int pageNumber, int pageSize, CancellationToken ct = default);
}