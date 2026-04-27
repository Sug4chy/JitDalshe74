using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Consultations;

namespace JitDalshe.Api.Admin.Requests;

public sealed record RequestFilter(
    RequestStatus? Status = null,
    DateOnly? StartDate = null,
    DateOnly? EndDate = null
);