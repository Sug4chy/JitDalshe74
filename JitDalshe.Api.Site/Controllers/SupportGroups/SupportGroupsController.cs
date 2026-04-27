using FluentValidation;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Models;
using JitDalshe.Api.Site.Controllers.SupportGroups.Requests;
using JitDalshe.Application.Site.UseCases.SupportGroups;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.SupportGroups;

[ApiController]
[Route("/api-site/v1/[controller]")]
public sealed partial class SupportGroupsController : ControllerBase
{
    [HttpPost]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUpForSupportGroup(
        [FromBody] SignUpForSupportGroupRequest request,
        [FromServices] ISignUpForSupportGroupUseCase signUp,
        CancellationToken ct = default)
    {
        var result = await signUp.SignUpAsync(
            applicantName: request.ApplicantName,
            applicantAge: request.ApplicantAge,
            applicantPhoneNumber: request.ApplicantPhoneNumber,
            applicantEmail: request.ApplicantEmail,
            communicationMethods: request.CommunicationMethod,
            ct: ct);

        return result.Match(
            _ => Created(),
            error => StatusCode(StatusCodes.Status500InternalServerError, ApiError.From(error.Message))
        );
    }
}