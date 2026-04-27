using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Volunteers;

public sealed class VolunteerRequest : AuditableEntity<IdOf<VolunteerRequest>>
{
    public string ApplicantName { get; init; }
    public int ApplicantAge { get; init; }
    public string? ApplicantPhoneNumber { get; init; }
    public string? ApplicantEmail { get; init; }
    public RequestStatus Status { get; private set; }
    public CommunicationMethod CommunicationMethods { get; init; }
    
    public string? Comment { get; private set; }

    private VolunteerRequest(
        IdOf<VolunteerRequest> id,
        string applicantName,
        int applicantAge,
        string? applicantPhoneNumber,
        string? applicantEmail,
        RequestStatus status,
        CommunicationMethod communicationMethods,
        string? comment = null)
    {
        Id = id;
        ApplicantName = applicantName;
        ApplicantAge = applicantAge;
        ApplicantPhoneNumber = applicantPhoneNumber;
        ApplicantEmail = applicantEmail;
        Status = status;
        CommunicationMethods = communicationMethods;
        Comment = comment;
    }

    public static VolunteerRequest Create(
        IdOf<VolunteerRequest> id,
        string applicantName,
        int applicantAge,
        string? applicantPhoneNumber,
        string? applicantEmail,
        CommunicationMethod communicationMethods)
        => new(id, applicantName, applicantAge, applicantPhoneNumber, applicantEmail, RequestStatus.New, communicationMethods);

    public void ChangeStatus(RequestStatus newStatus)
    {
        Status = newStatus;
    }

    public void UpdateComment(string? comment)
    {
        Comment = comment;
    }

    [UsedImplicitly]
#pragma warning disable CS8618
    private VolunteerRequest() { }
#pragma warning restore CS8618
}