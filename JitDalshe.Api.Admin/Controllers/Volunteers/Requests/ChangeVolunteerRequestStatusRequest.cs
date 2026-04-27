using System.Text.Json.Serialization;
using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Volunteers;

namespace JitDalshe.Api.Admin.Controllers.Volunteers.Requests;

[Validator<ChangeVolunteerRequestStatusRequestValidator>]
public sealed record ChangeVolunteerRequestStatusRequest(
    [property: JsonConverter(typeof(JsonStringEnumConverter))] 
    RequestStatus Status
);

public sealed class ChangeVolunteerRequestStatusRequestValidator : AbstractValidator<ChangeVolunteerRequestStatusRequest>
{
    public ChangeVolunteerRequestStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}