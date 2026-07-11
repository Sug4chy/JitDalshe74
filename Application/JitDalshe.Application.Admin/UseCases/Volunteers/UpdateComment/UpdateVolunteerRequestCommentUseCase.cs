using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Volunteers.UpdateComment;

[UseCase]
public sealed class UpdateVolunteerRequestCommentUseCase(IVolunteerRequestsRepository requests)
    : IUpdateVolunteerRequestCommentUseCase
{
    public async Task<UnitResult<Error>> UpdateAsync(
        IdOf<VolunteerRequest> requestId,
        string? comment,
        CancellationToken ct = default
    )
    {
        try
        {
            var maybeRequest = await requests.FindByIdAsync(requestId, ct);
            if (maybeRequest.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Заявка не найдена", ErrorGroup.NotFound));
            }

            var request = maybeRequest.Value;
            request.UpdateComment(comment);

            await requests.EditAsync(request, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure<Error>(Error.Of(e.Message));
        }
    }
}