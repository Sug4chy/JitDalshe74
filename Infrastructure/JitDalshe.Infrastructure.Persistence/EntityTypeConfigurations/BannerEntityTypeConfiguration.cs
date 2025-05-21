using JitDalshe.Domain.Entities.Banners;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class BannerEntityTypeConfiguration : IEntityTypeConfiguration<Banner>
{
    public void Configure(EntityTypeBuilder<Banner> builder)
    {
        builder.ToTable(nameof(Banner).ToSnakeCase());

        builder.HasId();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnName(nameof(Banner.Title).ToSnakeCase());

        builder.Property(x => x.IsClickable)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName(nameof(Banner.IsClickable).ToSnakeCase());

        builder.Property(x => x.RedirectOnClickUrl)
            .HasColumnName(nameof(Banner.RedirectOnClickUrl).ToSnakeCase());

        builder.Property(x => x.DisplayOrder)
            .HasColumnName(nameof(Banner.DisplayOrder).ToSnakeCase());
        builder.HasIndex(x => x.DisplayOrder).IsUnique();

        builder.HasAudits();

        builder.HasOne(x => x.Image)
            .WithOne(x => x.Banner)
            .HasForeignKey<BannerImage>(x => x.BannerId);
    }
}