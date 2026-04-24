using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Application.Admin.Dto;

public readonly record struct ConsultationRequestDto(
    Guid Id,
    string PatientName,
    int PatientAge,
    string? PatientPhoneNumber,
    string? PatientEmail,
    ConsultationRequestStatus ConsultationRequestStatus,
    PatientCommunicationMethod CommunicationMethods,
    DateOnly Date,
    string? Comment
);
