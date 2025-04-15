using JitDalshe.Api.Site.Attributes;
using JitDalshe.Api.Site.Controllers.Base;
using JitDalshe.Application.Api.UseCases.ListNews;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.News;

[ApiController]
[Route("/api/v1/news")]
public sealed class Controller : AbstractController
{
    [HttpGet]
    [ValidateRequest]
    public async Task<IActionResult> ListNews(
        [FromQuery] Requests.ListNewsRequest request,
        [FromServices] IListNewsUseCase useCase,
        CancellationToken ct = default
    )
    {
        var result = await useCase.ListAsync(request.PageNumber, request.PageSize, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }
}