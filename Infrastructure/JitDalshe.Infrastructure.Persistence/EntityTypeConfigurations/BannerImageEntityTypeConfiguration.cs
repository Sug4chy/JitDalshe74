using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class BannerImageEntityTypeConfiguration : IEntityTypeConfiguration<BannerImage>
{
    public void Configure(EntityTypeBuilder<BannerImage> builder)
    {
        builder.ToTable(nameof(BannerImage).ToSnakeCase());

        builder.HasId();

        builder.Property(x => x.Url)
            .IsRequired()
            .HasColumnName(nameof(BannerImage.Url).ToSnakeCase());

        builder.Property(x => x.ContentType)
            .IsRequired()
            .HasColumnName(nameof(BannerImage.ContentType).ToSnakeCase());

        builder.Property(x => x.BannerId)
            .IsRequired()
            .HasColumnName(nameof(BannerImage.BannerId).ToSnakeCase());

        builder.HasAudits();

        builder.HasOne(x => x.Banner)
            .WithOne(x => x.Image)
            .HasForeignKey<BannerImage>(x => x.BannerId);
    }
}