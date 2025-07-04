using JitDalshe.Domain.Entities.News;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class NewsPrimaryImageEntityTypeConfiguration : IEntityTypeConfiguration<NewsPrimaryImage>
{
    public void Configure(EntityTypeBuilder<NewsPrimaryImage> entity)
    {
        entity.ToTable(nameof(NewsPrimaryImage).ToSnakeCase());

        entity.HasKey(nameof(NewsPrimaryImage.NewsId), nameof(NewsPrimaryImage.NewsImageId));

        entity.Property(x => x.NewsId)
            .IsRequired()
            .ValueGeneratedNever()
            .HasGuidConversion()
            .HasColumnName(nameof(NewsPrimaryImage.NewsId).ToSnakeCase());

        entity.Property(x => x.NewsImageId)
            .IsRequired()
            .ValueGeneratedNever()
            .HasGuidConversion()
            .HasColumnName(nameof(NewsPrimaryImage.NewsImageId).ToSnakeCase());

        entity.HasOne(x => x.News)
            .WithOne(x => x.PrimaryImage)
            .HasForeignKey<NewsPrimaryImage>(x => x.NewsId)
            .OnDelete(DeleteBehavior.NoAction);

        entity.HasOne(x => x.NewsImage)
            .WithOne(x => x.PrimaryPhoto)
            .HasForeignKey<NewsPrimaryImage>(x => x.NewsImageId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}