using JitDalshe.Api.Models;
using JitDalshe.Application.Admin.Dto;
using JitDalshe.Application.Admin.UseCases.Reviews.ListUnmoderatedReviews;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Reviews;

[ApiController]
[Route("/admin-api/v1/[controller]")]
public sealed class ReviewsController : ControllerBase
{
    /// <summary>
    /// Получить все непромодерированные отзывы
    /// </summary>
    [HttpGet]
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
}