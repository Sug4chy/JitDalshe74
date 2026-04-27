using System.Text.Json.Serialization;
using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.SupportGroups;

namespace JitDalshe.Api.Admin.Controllers.SupportGroups.Requests;

[Validator<ChangeSupportGroupRequestStatusRequestValidator>]
public sealed record ChangeSupportGroupRequestStatusRequest(
    [property: JsonConverter(typeof(JsonStringEnumConverter))] 
    RequestStatus Status
);

public sealed class ChangeSupportGroupRequestStatusRequestValidator : AbstractValidator<ChangeSupportGroupRequestStatusRequest>
{
    public ChangeSupportGroupRequestStatusRequestValidator()
    {
        RuleFor(x => x.Status).IsInEnum();
    }
}