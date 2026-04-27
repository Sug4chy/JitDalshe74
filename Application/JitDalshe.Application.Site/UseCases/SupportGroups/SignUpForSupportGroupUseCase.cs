using JitDalshe.Application.Abstractions.Repositories;
using JitDalshe.Application.Attributes;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Application.Site.UseCases.SupportGroups;

[UseCase]
internal sealed class SignUpForSupportGroupUseCase : ISignUpForSupportGroupUseCase
{
    private readonly ISupportGroupRequestsRepository _supportGroupRepository;
    
    public SignUpForSupportGroupUseCase(
        ISupportGroupRequestsRepository supportGroupRepository
        )
    {
        _supportGroupRepository = supportGroupRepository;
    }

    public async Task<SignUpForSupportGroupResult> SignUpAsync(
        string applicantName,
        int applicantAge,
        string? applicantPhoneNumber,
        string? applicantEmail,
        CommunicationMethod communicationMethods,
        CancellationToken ct = default)
    {
        try
        {
            var request = SupportGroupRequest.Create(
                id: IdOf<SupportGroupRequest>.New(),
                applicantName: applicantName,
                applicantAge: applicantAge,
                applicantPhoneNumber: applicantPhoneNumber,
                applicantEmail: applicantEmail,
                communicationMethods: communicationMethods);
            
            await _supportGroupRepository.AddAsync(request, ct);
            
            return SignUpForSupportGroupResult.Success();
        }
        catch (Exception e)
        {
            return SignUpForSupportGroupResult.Failed(e.Message);
        }
    }
}