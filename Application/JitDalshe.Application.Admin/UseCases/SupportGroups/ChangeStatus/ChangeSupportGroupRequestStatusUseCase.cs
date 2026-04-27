using CSharpFunctionalExtensions;
using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Application.Errors;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Admin.UseCases.SupportGroups.ChangeStatus;

[UseCase]
public sealed class ChangeSupportGroupRequestStatusUseCase : IChangeSupportGroupRequestStatusUseCase
{
    private readonly ISupportGroupRequestsRepository _supportGroupRepository;

    public ChangeSupportGroupRequestStatusUseCase(ISupportGroupRequestsRepository supportGroupRepository)
    {
        _supportGroupRepository = supportGroupRepository;
    }
    
    public async Task<UnitResult<Error>> EditAsync(IdOf<SupportGroupRequest> requestId, RequestStatus newStatus, CancellationToken ct = default)
        
    {
        try
        {
            var maybeRequest = await _supportGroupRepository.FindByIdAsync(requestId, ct);
            if (maybeRequest.HasNoValue)
            {
                return UnitResult.Failure(Error.Of("Заявка не найдена", ErrorGroup.NotFound));
            }

            var request = maybeRequest.Value;
            
            request.ChangeStatus(newStatus);
            
            await _supportGroupRepository.EditAsync(request, ct);
            
            return UnitResult.Success<Error>();
        }
        catch (Exception e)
        {
            return UnitResult.Failure(Error.Of(e.Message));
        }
    }
}