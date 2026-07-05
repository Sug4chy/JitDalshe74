using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class BannerMobileImageEntityTypeConfiguration : IEntityTypeConfiguration<BannerMobileImage>
{
    public void Configure(EntityTypeBuilder<BannerMobileImage> entity)
    {
        entity.ToTable(nameof(BannerMobileImage).ToSnakeCase());

        entity.Property(x => x.Url)
            .IsRequired()
            .HasColumnName(nameof(BannerMobileImage.Url).ToSnakeCase());

        entity.Property(x => x.ContentType)
            .IsRequired()
            .HasColumnName(nameof(BannerMobileImage.ContentType).ToSnakeCase());

        entity.Property(x => x.BannerId)
            .IsRequired()
            .HasColumnName(nameof(BannerMobileImage.BannerId).ToSnakeCase());

        entity.HasOne(x => x.Banner)
            .WithOne(x => x.MobileImage)
            .HasForeignKey<BannerMobileImage>(x => x.BannerId);
    }
}