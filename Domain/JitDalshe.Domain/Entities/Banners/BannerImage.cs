using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Banners;

public sealed class BannerImage : AuditableEntity<IdOf<BannerImage>>
{
    public required string Url { get; init; }
    public required string ContentType { get; init; }

    public Banner? Banner { get; init; }
    public IdOf<Banner> BannerId { get; init; }
}