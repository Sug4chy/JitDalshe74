using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Events;

namespace JitDalshe.Application.Admin.UseCases.Events.CreateEvent;

public interface ICreateEventUseCase
{
    Task<UnitResult<Error>> CreateAsync(
        string title,
        string shortDescription,
        string fullText,
        DateOnly? date,
        TimeOnly? time,
        string? location,
        EventStatus status,
        string imageBase64Url,
        CancellationToken ct = default);
}