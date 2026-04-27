using CSharpFunctionalExtensions;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.SupportGroups.ChangeStatus;

public interface IChangeSupportGroupRequestStatusUseCase
{
    Task<UnitResult<Error>> EditAsync(IdOf<SupportGroupRequest> requestId, RequestStatus newStatus, CancellationToken ct = default);
}