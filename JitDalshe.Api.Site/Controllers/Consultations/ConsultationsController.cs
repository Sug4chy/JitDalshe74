using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Api.Site.Controllers.Consultations.Requests;
using JitDalshe.Application.Site.UseCases.Consultations;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.Consultations;

[ApiController]
[Route("/api-site/v1/[controller]")]
[ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
public sealed class ConsultationsController : AbstractController
{
    [HttpPost]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
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