using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Application.Admin.Dto;

public readonly record struct ConsultationRequestDto(
    Guid Id,
    string PatientName,
    int PatientAge,
    string? PatientPhoneNumber,
    string? PatientEmail,
    bool IsHandled,
    PatientCommunicationMethod CommunicationMethods,
    DateOnly Date
);
