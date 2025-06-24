using JitDalshe.Api.Admin.Controllers.Banners.Requests;
using JitDalshe.Api.Attributes;
using JitDalshe.Api.Controllers.Base;
using JitDalshe.Application.Admin.UseCases.Banners.CreateBanner;
using JitDalshe.Application.Admin.UseCases.Banners.DeleteBanner;
using JitDalshe.Application.Admin.UseCases.Banners.EditBanner;
using JitDalshe.Application.Admin.UseCases.Banners.ListBanners;
using JitDalshe.Application.Admin.UseCases.Banners.ReplaceBannerImage;
using JitDalshe.Application.Models;
using JitDalshe.Application.UseCases.Banners.GetBannerImage;
using JitDalshe.Application.UseCases.Banners.GetDisplayingBanners;
using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Domain.ValueObjects;
using Microsoft.AspNetCore.Mvc;

namespace JitDalshe.Api.Admin.Controllers.Banners;

[ApiController]
[Route("/admin-api/v1/[controller]")]
public sealed class BannersController : AbstractController
{
    /// <summary>
    /// Возвращает актуальные отображаемые на главной странице сайта баннеры
    /// </summary>
    [HttpGet("preview")]
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

    /// <summary>
    /// Возвращает изображение для баннера
    /// </summary>
    [HttpGet("{bannerId:guid}/image")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBannerImage(
        [FromRoute] Guid bannerId,
        [FromServices] IGetBannerImageUseCase getBannerImage,
        CancellationToken ct = default)
    {
        var result = await getBannerImage.GetAsync(IdOf<Banner>.From(bannerId), ct);

        return result.IsSuccess
            ? File(result.Value.ImageStream, result.Value.ContentType)
            : Error(result.Error);
    }

    /// <summary>
    /// Получение списка всех баннеров (от новых к старым)
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListBanners(
        [FromServices] IListBannersUseCase listBanners,
        CancellationToken ct = default)
    {
        var result = await listBanners.ListAsync(ct);

        return result.IsSuccess
            ? Ok(result.Value)
            : Error(result.Error);
    }

    [HttpPost]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBanner(
        [FromBody] CreateBannerRequest request,
        [FromServices] ICreateBannerUseCase createBanner,
        CancellationToken ct = default)
    {
        var result = await createBanner.CreateAsync(
            title: request.Title,
            imageBase64Url: request.ImageBase64Url,
            isClickable: request.IsClickable,
            redirectOnClickUrl: request.RedirectOnClickUrl,
            displayOrder: request.DisplayOrder,
            ct: ct);

        return result.IsSuccess
            ? CreatedAtAction("ListBanners", null)
            : Error(result.Error);
    }

    [HttpPatch("{id:guid}")]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EditBanner(
        [FromRoute] Guid id,
        [FromBody] EditBannerRequest request,
        [FromServices] IEditBannerUseCase editBanner,
        CancellationToken ct = default)
    {
        var result = await editBanner.EditAsync(
            bannerId: IdOf<Banner>.From(id),
            title: request.Title,
            isClickable: request.IsClickable,
            redirectOnClickUrl: request.RedirectOnClickUrl,
            displayOrder: request.DisplayOrder,
            ct: ct);

        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }

    [HttpPatch("{id:guid}/image")]
    [ValidateRequest]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ReplaceBannerImage(
        [FromRoute] Guid id,
        [FromBody] ReplaceBannerImageRequest request,
        [FromServices] IReplaceBannerImageUseCase replaceBannerImage,
        CancellationToken ct = default)
    {
        var result = await replaceBannerImage.ReplaceAsync(
            bannerId: IdOf<Banner>.From(id),
            imageBase64Url: request.ImageBase64Url,
            ct: ct);

        return result.IsSuccess
            ? Ok()
            : Error(result.Error);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteBanner(
        [FromRoute] Guid id,
        [FromServices] IDeleteBannerUseCase deleteBanner,
        CancellationToken ct = default)
    {
        var result = await deleteBanner.DeleteAsync(id: IdOf<Banner>.From(id), ct);

        return result.IsSuccess
            ? NoContent()
            : Error(result.Error);
    }
}