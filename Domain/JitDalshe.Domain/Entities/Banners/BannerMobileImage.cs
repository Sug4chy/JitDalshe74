using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Banners;

public sealed class BannerMobileImage : AuditableEntity<IdOf<BannerMobileImage>>, IImage
{
    public string Url { get; init; }
    public string ContentType { get; init; }

    public Banner? Banner { get; init; }
    public IdOf<Banner> BannerId { get; init; }

    private BannerMobileImage(
        IdOf<BannerMobileImage> id,
        string url,
        string contentType,
        IdOf<Banner> bannerId,
        Banner? banner = null)
    {
        Id = id;
        Url = url;
        ContentType = contentType;
        Banner = banner;
        BannerId = bannerId;
    }

    public static BannerMobileImage Create(
        IdOf<BannerMobileImage> id,
        string url,
        string contentType,
        IdOf<Banner> bannerId,
        Banner? banner = null)
        => new(id, url, contentType, bannerId, banner);

    [UsedImplicitly]
#pragma warning disable CS8618
    private BannerMobileImage()
    {
    }
#pragma warning restore CS8618
}