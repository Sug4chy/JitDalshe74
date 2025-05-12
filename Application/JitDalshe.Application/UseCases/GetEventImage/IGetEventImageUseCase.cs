using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;

namespace JitDalshe.Application.UseCases.GetEventImage;

public interface IGetEventImageUseCase
{
    Task<Result<EventImageModel, Error>> GetImageAsync(Guid eventId, CancellationToken ct = default);
}