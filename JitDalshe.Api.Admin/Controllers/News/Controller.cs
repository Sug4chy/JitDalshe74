using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.AdminApi.UseCases.ListNews;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.News;

[ApiController]
[Route("/admin-api/v1/[controller]")]
public sealed class Controller : AbstractController
{
    [HttpGet]
    public async Task<IActionResult> ListNews(
        [FromServices] IListNewsUseCase listNews,
        CancellationToken ct = default)
    {
        var result = await listNews.ListAsync(ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }
}