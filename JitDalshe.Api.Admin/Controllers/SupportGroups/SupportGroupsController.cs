using JitDalshe.Api.Admin.Controllers.SupportGroups.Requests;
using JitDalshe.Api.Admin.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.UseCases.SupportGroups.ChangeStatus;
using JitDalshe.Application.Admin.UseCases.SupportGroups.ListRequests;
using JitDalshe.Application.Admin.UseCases.SupportGroups.UpdateComment;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Common;
using JitDalshe.Domain.Entities.SupportGroups;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.SupportGroups;

[ApiController]
[Route("/api-admin/v1/[controller]")]
public class SupportGroupsController : AbstractController
{
    /// <summary>
    /// Получение полного списка заявок на консультацию (от новых к старым)
    /// </summary>
    [HttpGet]
    [ValidateRequest]
    [ProducesResponseType(typeof(PagedResult<SupportGroupRequestDto>),StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListSupportGroupRequests(
        [FromQuery] ListWithPaginationRequest request,
        [FromServices] IListSupportGroupRequestsUseCase listRequests,
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
    public async Task<IActionResult> EditSupportGroupRequest(
        [FromRoute] Guid id, 
        [FromBody] ChangeSupportGroupRequestStatusRequest request,
        [FromServices] IChangeSupportGroupRequestStatusUseCase changeRequestStatus,
        CancellationToken ct = default)
    {
        var result = await changeRequestStatus.EditAsync(IdOf<SupportGroupRequest>.From(id), request.Status, ct);
        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }
    
    [HttpPatch("{id:guid}/comment")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateSupportGroupComment(
        [FromRoute] Guid id,
        [FromBody] UpdateSupportGroupCommentRequest request,
        [FromServices] IUpdateSupportGroupRequestCommentUseCase updateComment,
        CancellationToken ct = default)
    {
        var result = await updateComment.UpdateAsync(
            IdOf<SupportGroupRequest>.From(id), 
            request.Comment, 
            ct);
    
        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }
}