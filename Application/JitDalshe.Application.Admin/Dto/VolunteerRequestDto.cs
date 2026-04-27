using JitDalshe.Domain.Common;

namespace JitDalshe.Application.Admin.Dto;

public readonly record struct VolunteerRequestDto(
    Guid Id,
    string ApplicantName,
    int ApplicantAge,
    string? ApplicantPhoneNumber,
    string? ApplicantEmail,
    RequestStatus Status,
    CommunicationMethod CommunicationMethods,
    DateOnly Date,
    string? Comment
);