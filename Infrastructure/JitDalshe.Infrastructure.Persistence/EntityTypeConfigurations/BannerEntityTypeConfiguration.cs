using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class BannerEntityTypeConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> entity)
    {
        entity.ToTable(nameof(Banner).ToSnakeCase());

        entity.HasId();

        entity.Property(x => x.Title)
            .IsRequired()
            .HasColumnName(nameof(Banner.Title).ToSnakeCase());

        entity.Property(x => x.IsClickable)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName(nameof(Banner.IsClickable).ToSnakeCase());

        entity.Property(x => x.RedirectOnClickUrl)
            .HasColumnName(nameof(Banner.RedirectOnClickUrl).ToSnakeCase());

        entity.Property(x => x.DisplayOrder)
            .HasColumnName(nameof(Banner.DisplayOrder).ToSnakeCase());
        entity.HasIndex(x => x.DisplayOrder).IsUnique();

        entity.HasAudits();

        entity.HasOne(x => x.Image)
            .WithOne(x => x.Banner)
            .HasForeignKey<BannerImage>(x => x.BannerId);
    }
}