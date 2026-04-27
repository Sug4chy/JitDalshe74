using JitDalshe.Api.Attributes;
using JitDalshe.Api.Models;
using JitDalshe.Api.Site.Controllers.Consultations.Requests;
using JitDalshe.Application.Site.UseCases.Consultations;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.Consultations;

[ApiController]
[Route("/api-site/v1/[controller]")]
public sealed class ConsultationsController : ControllerBase
{
    [HttpPost]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SignUpForConsultation(
        [FromBody] SignUpForConsultationRequest request,
        [FromServices] ISignUpForConsultationUseCase signUpForConsultation,
        CancellationToken ct = default)
    {
        var result = await signUpForConsultation.SignUpAsync(
            patientName: request.PatientName,
            patientAge: request.PatientAge,
            patientPhoneNumber: request.PatientPhoneNumber,
            patientEmail: request.PatientEmail,
            communicationMethod: request.CommunicationMethod,
            ct: ct);

        return result.Match(
            _ => Created(),
            error => StatusCode(StatusCodes.Status500InternalServerError, ApiError.From(error.Message))
        );
    }
}