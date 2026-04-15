using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.UseCases.Consultations.ChangeStatus;
using JitDalshe.Application.Admin.UseCases.Consultations.ListRequests;
using JitDalshe.Domain.Entities.Consultations;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Consultations;

[ApiController]
[Route("/api-admin/v1/[controller]")]
public class ConsultationsController : AbstractController
{
    /// <summary>
    /// Получение полного списка заявок на консультацию (от новых к старым)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ConsultationRequestDto[]),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListConsultationRequests(
        [FromServices] IListConsultationRequestsUseCase listRequests,
        CancellationToken ct = default)
    {
        var result = await listRequests.ListAsync(ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }

    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditConsultationRequest(
        [FromRoute] Guid id, 
        [FromServices] IChangeConsultationRequestStatusUseCase changeRequestStatus,
        CancellationToken ct = default)
    {
        var result = await changeRequestStatus.EditAsync(IdOf<ConsultationRequest>.From(id), ct);
        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }
}