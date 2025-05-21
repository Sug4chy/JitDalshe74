using JitDalshe.Application.Models;
using JitDalshe.Domain.Entities.Banners;

namespace JitDalshe.Application.Extensions;

public static class DomainToModelMappingExtensions
{
    public static DisplayingBannerModel ToDisplayingModel(this Banner banner)
        => new(
            Title: banner.Title,
            ImageUrl: banner.Image!.Url,
            DisplayOrder: banner.DisplayOrder!.Value,
            RedirectOnClickUrl: banner.RedirectOnClickUrl
        );
}