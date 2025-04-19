using JitDalshe.Domain.Entities;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class NewsPhotoEntityTypeConfiguration : IEntityTypeConfiguration<NewsPhoto>
{
    public void Configure(EntityTypeBuilder<NewsPhoto> builder)
    {
        builder.ToTable(nameof(NewsPhoto).ToSnakeCase());

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever()
            .HasGuidConversion()
            .HasColumnName(nameof(NewsPhoto.Id).ToSnakeCase());

        builder.Property(x => x.ExtId)
            .IsRequired()
            .HasColumnName(nameof(NewsPhoto.ExtId).ToSnakeCase());
        builder.HasIndex(x => x.ExtId).IsUnique();

        builder.Property(x => x.Uri)
            .IsRequired()
            .HasConversion<string>(
                x => x.ToString(),
                x => new Uri(x))
            .HasColumnName(nameof(NewsPhoto.Uri).ToSnakeCase());

        builder.Property(x => x.NewsId)
            .IsRequired()
            .HasGuidConversion()
            .HasColumnName(nameof(NewsPhoto.NewsId).ToSnakeCase());

        builder.HasAudits();

        builder.HasOne(x => x.News)
            .WithMany(x => x.Photos)
            .HasForeignKey(x => x.NewsId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.PrimaryPhoto)
            .WithOne(x => x.NewsPhoto)
            .HasForeignKey<NewsPrimaryPhoto>(x => x.NewsPhotoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}