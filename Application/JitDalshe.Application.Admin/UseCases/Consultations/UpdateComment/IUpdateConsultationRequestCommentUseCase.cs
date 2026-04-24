using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.Consultations.UpdateComment;

public interface IUpdateConsultationRequestCommentUseCase
{
    Task<UnitResult<Error>> UpdateAsync(IdOf<ConsultationRequest> requestId, string? comment, CancellationToken ct = default);
}