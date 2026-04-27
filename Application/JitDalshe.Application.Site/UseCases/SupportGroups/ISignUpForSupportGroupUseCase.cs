using JitDalshe.Domain.Common;

namespace JitDalshe.Application.Site.UseCases.SupportGroups;

public interface ISignUpForSupportGroupUseCase
{
    Task<SignUpForSupportGroupResult> SignUpAsync(
        string applicantName,
        int applicantAge,
        string? applicantPhoneNumber,
        string? applicantEmail,
        CommunicationMethod communicationMethods,
        CancellationToken ct = default);
}