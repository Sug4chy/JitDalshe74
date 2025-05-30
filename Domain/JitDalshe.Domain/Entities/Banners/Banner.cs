using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Banners;

public sealed class Banner : AuditableEntity<IdOf<Banner>>
{
    private int? _displayOrder;

    public required string Title { get; init; }
    public bool IsClickable { get; init; }
    public string? RedirectOnClickUrl { get; init; }

    public int? DisplayOrder
    {
        get => _displayOrder;
        set
        {
            if (value is <= 0 or > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(DisplayOrder), "Display order must be between 0 and 5");
            }

            _displayOrder = value;
        }
    }

    public BannerImage? Image { get; init; }

    public Banner(IdOf<Banner> id)
    {
        Id = id;
    }
}