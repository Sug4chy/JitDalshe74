using JitDalshe.Api.Models;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.UseCases.Reviews.DeleteReview;
using JitDalshe.Application.Admin.UseCases.Reviews.ListUnmoderatedReviews;
using JitDalshe.Application.Admin.UseCases.Reviews.ModerateReview;
using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Reviews;

[ApiController]
[Route("/api-admin/v1/[controller]")]
public sealed class ReviewsController : ControllerBase
{
    /// <summary>
    /// Получить все непромодерированные отзывы
    /// </summary>
    [HttpGet("unmoderated")]
    [ProducesResponseType(typeof(UnmoderatedReviewDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListUnmoderatedReviews(
        [FromServices] IListUnmoderatedReviewsUseCase listUnmoderatedReviews,
        CancellationToken ct = default)
    {
        var result = await listUnmoderatedReviews.ListAsync(ct);

        return result.Match(
            found => Ok(found.Reviews),
            error => StatusCode(StatusCodes.Status500InternalServerError, ApiError.From(error.Message))
        );
    }

    /// <summary>
    /// Одобрить отзыв
    /// </summary>
    [HttpPost("{id:guid}/moderate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ModerateReview(
        [FromRoute] Guid id,
        [FromServices] IModerateReviewUseCase moderateReview,
        CancellationToken ct = default)
    {
        var result = await moderateReview.ModerateAsync(IdOf<Review>.From(id), ct);

        return result.Match<IActionResult>(
            _ => Ok(),
            _ => NotFound(ApiError.From($"Review with ID {id} wasn't found")),
            _ => Conflict(ApiError.From($"Review with ID {id} is already moderated")),
            error => StatusCode(StatusCodes.Status500InternalServerError, ApiError.From(error.Message))
        );
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