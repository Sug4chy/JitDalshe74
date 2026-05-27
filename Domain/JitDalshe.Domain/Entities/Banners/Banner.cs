using JetBrains.Annotations;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;

namespace JitDalshe.Domain.Entities.Banners;

public sealed class Banner : AuditableEntity<IdOf<Banner>>
{
    private int? _displayOrder;

    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool IsClickable { get; set; }
    public string? RedirectOnClickUrl { get; set; }
    public BannerStatus Status { get; set; }
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
    public BannerMobileImage? MobileImage { get; init; }
    
    private Banner(
        IdOf<Banner> id,
        string? title,
        string? description,
        string? redirectOnClickUrl,
        int? displayOrder,
        BannerImage? image,
        BannerMobileImage? mobileImage,
        BannerStatus status)
    {
        Id = id;
        Title = title;
        Description = description;
        IsClickable = redirectOnClickUrl is not null;
        RedirectOnClickUrl = redirectOnClickUrl;
        DisplayOrder = displayOrder;
        Image = image;
        MobileImage = mobileImage;
        Status = status;
    }

    public static Banner Create(
        IdOf<Banner> id, 
        string? title, 
        string? description,
        string? redirectOnClickUrl, 
        int? displayOrder,
        BannerImage? image,
        BannerMobileImage? mobileImage,
        BannerStatus status = BannerStatus.NotPublished) 
        => new(id, title, description, redirectOnClickUrl, displayOrder, image, mobileImage, status);

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