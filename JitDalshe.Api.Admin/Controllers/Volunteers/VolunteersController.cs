using JitDalshe.Api.Admin.Controllers.Volunteers.Requests;
using JitDalshe.Api.Admin.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.UseCases.Volunteers.ChangeStatus;
using JitDalshe.Application.Admin.UseCases.Volunteers.ListRequests;
using JitDalshe.Application.Admin.UseCases.Volunteers.UpdateComment;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.Volunteers;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Volunteers;

[ApiController]
[Route("/api-admin/v1/[controller]")]
public class VolunteersController : AbstractController
{
    /// <summary>
    /// Получение полного списка заявок на консультацию (от новых к старым)
    /// </summary>
    [HttpGet]
    [ValidateRequest]
    [ProducesResponseType(typeof(PagedResult<VolunteerRequestDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListVolunteerRequests(
        [FromQuery] ListWithPaginationRequest request,
        [FromServices] IListVolunteerRequestsUseCase listRequests,
        [FromQuery] RequestStatus? status = null,
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
    public async Task<IActionResult> EditVolunteerRequest(
        [FromRoute] Guid id, 
        [FromBody] ChangeVolunteerRequestStatusRequest request,
        [FromServices] IChangeVolunteerRequestStatusUseCase changeRequestStatus,
        CancellationToken ct = default)
    {
        var result = await changeRequestStatus.EditAsync(IdOf<VolunteerRequest>.From(id), request.Status, ct);
        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }
    
    [HttpPatch("{id:guid}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateVolunteerComment(
        [FromRoute] Guid id,
        [FromBody] UpdateVolunteerCommentRequest request,
        [FromServices] IUpdateVolunteerRequestCommentUseCase updateComment,
        CancellationToken ct = default)
    {
        var result = await updateComment.UpdateAsync(
            IdOf<VolunteerRequest>.From(id), 
            request.Comment, 
            ct);
    
        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }
}