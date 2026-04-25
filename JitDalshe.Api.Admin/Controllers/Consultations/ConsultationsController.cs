using JitDalshe.Api.Admin.Controllers.Consultations.Requests;
using JitDalshe.Api.Admin.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.UseCases.Consultations.ChangeStatus;
using JitDalshe.Application.Admin.UseCases.Consultations.ListRequests;
using JitDalshe.Application.Admin.UseCases.Consultations.UpdateComment;
using JitDalshe.Application.Models;
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
    [ValidateRequest]
    [ProducesResponseType(typeof(PagedResult<ConsultationRequestDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListConsultationRequests(
        [FromQuery] ListWithPaginationRequest request,
        [FromServices] IListConsultationRequestsUseCase listRequests,
        [FromQuery] ConsultationRequestStatus? status = null,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null,
        CancellationToken ct = default)
    {
        var result = await listRequests.ListAsync(
            request.PageNumber,
            request.PageSize,
            status, 
            startDate, 
            endDate, 
            ct);


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
        [FromBody] ChangeConsultationRequestStatusRequest request,
        [FromServices] IChangeConsultationRequestStatusUseCase changeRequestStatus,
        CancellationToken ct = default)
    {
        var result = await changeRequestStatus.EditAsync(IdOf<ConsultationRequest>.From(id), request.Status, ct);
        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }
    
    [HttpPatch("{id:guid}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateConsultationComment(
        [FromRoute] Guid id,
        [FromBody] UpdateConsultationCommentRequest request,
        [FromServices] IUpdateConsultationRequestCommentUseCase updateComment,
        CancellationToken ct = default)
    {
        var result = await updateComment.UpdateAsync(
            IdOf<ConsultationRequest>.From(id), 
            request.Comment, 
            ct);
    
        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }
}