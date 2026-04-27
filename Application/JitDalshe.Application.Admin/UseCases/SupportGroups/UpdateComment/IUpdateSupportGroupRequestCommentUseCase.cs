using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.SupportGroups.UpdateComment;

public interface IUpdateSupportGroupRequestCommentUseCase
{
    Task<UnitResult<Error>> UpdateAsync(IdOf<SupportGroupRequest> requestId, string? comment, CancellationToken ct = default);
}