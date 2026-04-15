using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Consultations;

public sealed class ConsultationRequest : AuditableEntity<IdOf<ConsultationRequest>>
{
    public string PatientName { get; init; }
    public int PatientAge { get; init; }
    public string? PatientPhoneNumber { get; init; }
    public string? PatientEmail { get; init; }
    public bool IsHandled { get; private set; }
    public PatientCommunicationMethod CommunicationMethods { get; init; }
    
    private ConsultationRequest(
        IdOf<ConsultationRequest> id,
        string patientName,
        int patientAge,
        string? patientPhoneNumber,
        string? patientEmail,
        bool isHandled,
        PatientCommunicationMethod communicationMethods)
    {
        Id = id;
        PatientName = patientName;
        PatientAge = patientAge;
        PatientPhoneNumber = patientPhoneNumber;
        PatientEmail = patientEmail;
        IsHandled = isHandled;
        CommunicationMethods = communicationMethods;
    }

    public static ConsultationRequest Create(
        IdOf<ConsultationRequest> id,
        string patientName,
        int patientAge,
        string? patientPhoneNumber,
        string? patientEmail,
        bool isHandled,
        PatientCommunicationMethod communicationMethods)
        => new(id, patientName, patientAge, patientPhoneNumber, patientEmail, isHandled, communicationMethods);

    public void ToggleHandledStatus()
    {
        IsHandled = !IsHandled;
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