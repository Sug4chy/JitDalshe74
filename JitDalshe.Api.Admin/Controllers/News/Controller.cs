using JitDalshe.Api.Admin.Controllers.News.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Admin.UseCases.DeleteNews;
using JitDalshe.Application.Admin.UseCases.EditNews;
using JitDalshe.Application.Admin.UseCases.ListNews;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.News;

[ApiController]
[Route("/admin-api/v1/news")]
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

    [HttpPut("{id:guid}")]
    [ValidateRequest]
    public async Task<IActionResult> EditNews(
        [FromRoute] Guid id,
        [FromBody] EditNewsRequest request,
        [FromServices] IEditNewsUseCase editNews,
        CancellationToken ct = default)
    {
        var result = await editNews.EditAsync(
            newsId: IdOf<Domain.Entities.News>.From(id),
            text: request.Text,
            primaryPhotoId: IdOf<Domain.Entities.NewsPhoto>.From(request.PrimaryPhotoId ?? Guid.Empty),
            ct: ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteNews(
        [FromRoute] Guid id,
        [FromServices] IDeleteNewsUseCase deleteNews,
        CancellationToken ct = default
    )
    {
        var result = await deleteNews.DeleteAsync(IdOf<Domain.Entities.News>.From(id), ct);

        return result.IsSuccess
            ? NoContent()
            : Error(result.Error);
    }
}