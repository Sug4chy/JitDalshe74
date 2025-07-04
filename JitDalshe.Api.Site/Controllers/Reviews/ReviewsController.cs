using JitDalshe.Api.Attributes;
using JitDalshe.Api.Site.Requests;
using JitDalshe.Application.Site.UseCases.ListReviews;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.Reviews;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class ReviewsController : ControllerBase
{
    [HttpGet]
    [ValidateRequest]
    public async Task<IActionResult> ListReviews(
        [FromQuery] ListWithPaginationRequest request,
        [FromServices] IListReviewsUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.ListAsync(request.PageNumber, request.PageSize, ct);

        return result.Match(
            found => Ok(found.Reviews),
            error => StatusCode(StatusCodes.Status500InternalServerError, error.InternalError));
    }
}