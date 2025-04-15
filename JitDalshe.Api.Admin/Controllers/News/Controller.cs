using JitDalshe.Api.Admin.Controllers.News.Requests;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Admin.UseCases.EditNews;
using JitDalshe.Application.Admin.UseCases.ListNews;
using JitDalshe.Domain.ValueObjects;
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

    [HttpPut("{Id:guid}")]
    public async Task<IActionResult> EditNews(
        [FromBody] EditNewsRequest request,
        [FromServices] IEditNewsUseCase editNews,
        CancellationToken ct = default)
    {
        var result = await editNews.EditAsync(
            newsId: IdOf<Domain.Entities.News>.From(request.Id), 
            text: request.Text,
            primaryPhotoId: IdOf<Domain.Entities.NewsPhoto>.From(request.Id),
            ct: ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }
}