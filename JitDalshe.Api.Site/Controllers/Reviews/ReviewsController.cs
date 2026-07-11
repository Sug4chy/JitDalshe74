using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Models;
using JitDalshe.Api.Site.Controllers.Reviews.Requests;
using JitDalshe.Api.Site.Requests;
using JitDalshe.Application.Site.Dto;
using JitDalshe.Application.Site.UseCases.Reviews.LeaveReview;
using JitDalshe.Application.Site.UseCases.Reviews.ListReviews;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.Reviews;

[ApiController]
[Route("/api-site/v1/[controller]")]
public sealed class ReviewsController : AbstractController
{
    /// <summary>
    /// Получить отзывы по страницам (от новых к старым)
    /// </summary>
    [HttpGet]
    [ValidateRequest]
    [ProducesResponseType(typeof(ReviewDto[]), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListReviews(
        [FromQuery] ListWithPaginationRequest request,
        [FromServices] IListReviewsUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.ListAsync(request.PageNumber, request.PageSize, ct);

        return result.Match(
            found => Ok(found.Reviews),
            error => StatusCode(StatusCodes.Status500InternalServerError, ApiError.From(error.InternalError)));
    }

    /// <summary>
    /// Оставить отзыв
    /// </summary>
    [HttpPost]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiError), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> LeaveReview(
        [FromBody] LeaveReviewRequest request,
        [FromServices] ILeaveReviewUseCase leaveReview,
        CancellationToken ct = default)
    {
        var result = await leaveReview.LeaveAsync(
            reviewerName: request.ReviewerName,
            reviewerAge: request.ReviewerAge,
            reviewerStatus: request.ReviewerStatus,
            text: request.Text,
            ct: ct);

        return result.Match(
            _ => Created(),
            error => StatusCode(StatusCodes.Status500InternalServerError, ApiError.From(error.Message))
        );
    }
}