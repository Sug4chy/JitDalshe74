using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Banners;

public sealed class Banner : AuditableEntity<IdOf<Banner>>
{
    private int? _displayOrder;

    public string Title { get; set; }
    public bool IsClickable { get; set; }
    public string? RedirectOnClickUrl { get; set; }

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

    private Banner(
        IdOf<Banner> id,
        string title,
        string? redirectOnClickUrl,
        int? displayOrder,
        BannerImage? image)
    {
        Id = id;
        Title = title;
        IsClickable = redirectOnClickUrl is not null;
        RedirectOnClickUrl = redirectOnClickUrl;
        DisplayOrder = displayOrder;
        Image = image;
    }

    public static Banner Create(
        IdOf<Banner> id, 
        string title, 
        string? redirectOnClickUrl, 
        int? displayOrder,
        BannerImage? image) 
        => new(id, title, redirectOnClickUrl, displayOrder, image);

    /// <summary>
    /// For EF Core
    /// </summary>
    [UsedImplicitly]
#pragma warning disable CS8618
    private Banner()
    {
    }
#pragma warning restore CS8618
}