using JitDalshe.Domain.Entities.News;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class NewsImageEntityTypeConfiguration : IEntityTypeConfiguration<NewsImage>
{
    public void Configure(EntityTypeBuilder<NewsImage> builder)
    {
        builder.ToTable(nameof(NewsImage).ToSnakeCase());

        builder.HasId();

        builder.Property(x => x.ExtId)
            .IsRequired()
            .HasColumnName(nameof(NewsImage.ExtId).ToSnakeCase());
        builder.HasIndex(x => x.ExtId).IsUnique();

        builder.Property(x => x.Url)
            .IsRequired()
            .HasConversion<string>(
                x => x.ToString(),
                x => new Uri(x))
            .HasColumnName(nameof(NewsImage.Url).ToSnakeCase());

        builder.Property(x => x.NewsId)
            .IsRequired()
            .HasGuidConversion()
            .HasColumnName(nameof(NewsImage.NewsId).ToSnakeCase());

        builder.HasAudits();

        builder.HasOne(x => x.News)
            .WithMany(x => x.Images)
            .HasForeignKey(x => x.NewsId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.PrimaryPhoto)
            .WithOne(x => x.NewsImage)
            .HasForeignKey<NewsPrimaryImage>(x => x.NewsImageId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}