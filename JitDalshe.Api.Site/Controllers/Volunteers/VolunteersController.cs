using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Api.Site.Controllers.Volunteers.Requests;
using JitDalshe.Application.Site.UseCases.Volunteers;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.Volunteers;

[ApiController]
[Route("/api-site/v1/[controller]")]
public sealed class VolunteersController : AbstractController
{
    [HttpPost]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUpForVolunteer(
        [FromBody] SignUpForVolunteerRequest request,
        [FromServices] ISignUpForVolunteerUseCase signUp,
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