using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Application.Models;

namespace JitDalshe.Application.UseCases.Events.GetEventImage;

public interface IGetEventImageUseCase
{
    Task<Result<ImageModel, Error>> GetImageAsync(Guid eventId, CancellationToken ct = default);
}