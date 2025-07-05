using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class BannerImageEntityTypeConfiguration : IEntityTypeConfiguration<BannerImage>
{
    public void Configure(EntityTypeBuilder<BannerImage> entity)
    {
        entity.ToTable(nameof(BannerImage).ToSnakeCase());

        entity.HasId();

        entity.Property(x => x.Url)
            .IsRequired()
            .HasColumnName(nameof(BannerImage.Url).ToSnakeCase());

        entity.Property(x => x.ContentType)
            .IsRequired()
            .HasColumnName(nameof(BannerImage.ContentType).ToSnakeCase());

        entity.Property(x => x.BannerId)
            .IsRequired()
            .HasColumnName(nameof(BannerImage.BannerId).ToSnakeCase());

        entity.HasAudits();

        entity.HasOne(x => x.Banner)
            .WithOne(x => x.Image)
            .HasForeignKey<BannerImage>(x => x.BannerId);
    }
}