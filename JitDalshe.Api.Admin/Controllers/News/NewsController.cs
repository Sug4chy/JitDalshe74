using JitDalshe.Api.Admin.Controllers.News.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Admin.UseCases.News.DeleteNews;
using JitDalshe.Application.Admin.UseCases.News.EditNews;
using JitDalshe.Application.Admin.UseCases.News.ListNews;
using JitDalshe.Domain.Entities.News;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.News;

[ApiController]
[Route("/admin-api/v1/[controller]")]
public sealed class NewsController : AbstractController
{
    /// <summary>
    /// Получение списка всех новостей (от новых к старым)
    /// </summary>
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

    /// <summary>
    /// Редактирование новости (текста и основного изображения) по ID
    /// </summary>
    [HttpPut("{id:guid}")]
    [ValidateRequest]
    public async Task<IActionResult> EditNews(
        [FromRoute] Guid id,
        [FromBody] EditNewsRequest request,
        [FromServices] IEditNewsUseCase editNews,
        CancellationToken ct = default)
    {
        var result = await editNews.EditAsync(
            newsId: IdOf<Domain.Entities.News.News>.From(id),
            text: request.Text,
            primaryPhotoId: IdOf<NewsImage>.From(request.PrimaryPhotoId ?? Guid.Empty),
            ct: ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }

    /// <summary>
    /// Удаление новости по ID
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteNews(
        [FromRoute] Guid id,
        [FromServices] IDeleteNewsUseCase deleteNews,
        CancellationToken ct = default
    )
    {
        var result = await deleteNews.DeleteAsync(IdOf<Domain.Entities.News.News>.From(id), ct);

        return result.IsSuccess
            ? NoContent()
            : Error(result.Error);
    }
}