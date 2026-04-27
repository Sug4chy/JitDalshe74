using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Consultations;

public sealed class ConsultationRequest : AuditableEntity<IdOf<ConsultationRequest>>
{
    public string PatientName { get; init; }
    public int PatientAge { get; init; }
    public string? PatientPhoneNumber { get; init; }
    public string? PatientEmail { get; init; }
    public RequestStatus ConsultationRequestStatus { get; private set; }
    public CommunicationMethod CommunicationMethods { get; init; }
    
    public string? Comment { get; private set; }
    
    private ConsultationRequest(
        IdOf<ConsultationRequest> id,
        string patientName,
        int patientAge,
        string? patientPhoneNumber,
        string? patientEmail,
        RequestStatus consultationRequestStatus,
        CommunicationMethod communicationMethods,
        string? comment = null)
    {
        Id = id;
        PatientName = patientName;
        PatientAge = patientAge;
        PatientPhoneNumber = patientPhoneNumber;
        PatientEmail = patientEmail;
        ConsultationRequestStatus = consultationRequestStatus;
        CommunicationMethods = communicationMethods;
        Comment = comment;
    }

    public static ConsultationRequest Create(
        IdOf<ConsultationRequest> id,
        string patientName,
        int patientAge,
        string? patientPhoneNumber,
        string? patientEmail,
        CommunicationMethod communicationMethods)
        => new(id, patientName, patientAge, patientPhoneNumber, patientEmail, RequestStatus.New, communicationMethods);

    public void ChangeStatus(RequestStatus newStatus)
    {
        ConsultationRequestStatus = newStatus;
    }
    
    public void UpdateComment(string? comment)
    {
        Comment = comment;    
    }
    
    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private ConsultationRequest()
    {
    }
#pragma warning restore CS8618
}