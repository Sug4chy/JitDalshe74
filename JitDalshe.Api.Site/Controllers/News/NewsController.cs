using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Api.Site.Controllers.News.Requests;
using JitDalshe.Application.Site.UseCases.ListNews;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Site.Controllers.News;

[ApiController]
[Route("/api/v1/[controller]")]
public sealed class NewsController : AbstractController
{
    /// <summary>
    /// Получение новостей с пагинацией (от новых к старым)
    /// </summary>
    [HttpGet]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListNews(
        [FromQuery] ListNewsRequest request,
        [FromServices] IListNewsUseCase useCase,
        CancellationToken ct = default)
    {
        var result = await useCase.ListAsync(request.PageNumber, request.PageSize, ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }
}