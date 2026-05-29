using JitDalshe.Api.Admin.Controllers.Reviews.Requests;
using JitDalshe.Api.Admin.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.UseCases.Reviews.ChangeStatus;
using JitDalshe.Application.Admin.UseCases.Reviews.DeleteReview;
using JitDalshe.Application.Admin.UseCases.Reviews.ListReviews;
using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Reviews;

[ApiController]
[Route("/api-admin/v1/[controller]")]
public sealed class ReviewsController : AbstractController
{
    /// <summary>
    /// Получить отзывы с пагинацией и фильтрацией по статусу
    /// </summary>
    [HttpGet]
    [ValidateRequest]
    [ProducesResponseType(typeof(PagedResult<ReviewDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListReviews(
        [FromQuery] ListWithPaginationRequest request,
        [FromServices] IListReviewsUseCase listReviews,
        [FromQuery] ReviewStatus? status = null,
        CancellationToken ct = default)
    {
        var result = await listReviews.ListAsync(request.PageNumber, request.PageSize, status, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }


    /// <summary>
    /// Изменить статус отзыва (New, InProgress, Published, NotPublished)
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ChangeReviewStatus(
        [FromRoute] Guid id,
        [FromBody] ChangeReviewStatusRequest request,
        [FromServices] IChangeReviewStatusUseCase changeStatus,
        CancellationToken ct = default)
    {
        var result = await changeStatus.ChangeStatusAsync(IdOf<Review>.From(id), request.Status, ct);

        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }

    /// <summary>
    /// Удалить отзыв
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteReview(
        [FromRoute] Guid id,
        [FromServices] IDeleteReviewUseCase deleteReview,
        CancellationToken ct = default)
    {
        var result = await deleteReview.DeleteAsync(IdOf<Review>.From(id), ct);

        return result.Match<IActionResult>(
            _ => NoContent(),
            _ => NotFound(ApiError.From($"Review with ID {id} wasn't found")),
            error => StatusCode(StatusCodes.Status500InternalServerError, ApiError.From(error.Message))
        );
    }
}