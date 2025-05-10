using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Site.Models;

namespace JitDalshe.Application.Site.UseCases.GetEventImage;

public interface IGetEventImageUseCase
{
    Task<Result<EventImageModel, Error>> GetImageAsync(Guid eventId, CancellationToken ct = default);
}