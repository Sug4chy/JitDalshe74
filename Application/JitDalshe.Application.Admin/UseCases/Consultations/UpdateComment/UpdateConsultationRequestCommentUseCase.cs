using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Consultations.UpdateComment;


[UseCase]
public sealed class UpdateConsultationRequestCommentUseCase : IUpdateConsultationRequestCommentUseCase
{
    private readonly IConsultationRequestsRepository _requests;
    
    public UpdateConsultationRequestCommentUseCase(IConsultationRequestsRepository requests)
    {
        _requests = requests;
    }

    public async Task<UnitResult<Error>> UpdateAsync(
        IdOf<ConsultationRequest> requestId,
        string? comment,
        CancellationToken ct = default
        )
    {
        try
        {
            var maybeRequest = await _requests.FindByIdAsync(requestId, ct);
            if (maybeRequest.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Заявка не найдена", ErrorGroup.NotFound));
            }

            var request = maybeRequest.Value;
            request.UpdateComment(comment);

            await _requests.EditAsync(request, ct);

            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure<Error>(Error.Of(e.Message));
        }
    }
}