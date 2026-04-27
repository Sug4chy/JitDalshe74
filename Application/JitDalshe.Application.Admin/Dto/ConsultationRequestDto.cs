using JitDalshe.Domain.Common;


namespace JitDalshe.Application.Admin.Dto;

public readonly record struct ConsultationRequestDto(
    Guid Id,
    string PatientName,
    int PatientAge,
    string? PatientPhoneNumber,
    string? PatientEmail,
    RequestStatus RequestStatus,
    CommunicationMethod CommunicationMethods,
    DateOnly Date,
    string? Comment
);
