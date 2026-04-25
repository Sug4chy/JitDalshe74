using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Api.Admin.Requests;

public sealed record ConsultationRequestFilter(
    ConsultationRequestStatus? Status = null,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null
);