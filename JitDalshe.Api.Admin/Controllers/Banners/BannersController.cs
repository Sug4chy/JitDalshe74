using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Models;
using JitDalshe.Application.UseCases.GetDisplayingBanners;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Banners;

[ApiController]
[Route("/admin-api/v1/[controller]")]
public sealed class BannersController : AbstractController
{
    /// <summary>
    /// Возвращает актуальные отображаемые на главной странице сайта баннеры
    /// </summary>
    [HttpGet("displaying")]
    [ProducesResponseType(typeof(DisplayingBannerModel[]), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDisplayingBanners(
        [FromServices] IGetDisplayingBannersUseCase getDisplayingBanners,
        CancellationToken ct = default)
    {
        var result = await getDisplayingBanners.GetAsync(ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }
}