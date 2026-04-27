using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Volunteers.UpdateComment;

public interface IUpdateVolunteerRequestCommentUseCase
{
    Task<UnitResult<Error>> UpdateAsync(IdOf<VolunteerRequest> requestId, string? comment, CancellationToken ct = default);
}