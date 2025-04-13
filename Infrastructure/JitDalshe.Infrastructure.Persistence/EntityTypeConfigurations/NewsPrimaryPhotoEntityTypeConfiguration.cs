using JitDalse.Domain.Entities;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class NewsPrimaryPhotoEntityTypeConfiguration : IEntityTypeConfiguration<NewsPrimaryPhoto>
{
    public void Configure(EntityTypeBuilder<NewsPrimaryPhoto> builder)
    {
        builder.ToTable(nameof(NewsPrimaryPhoto).ToSnakeCase());

        builder.HasKey(nameof(NewsPrimaryPhoto.NewsId), nameof(NewsPrimaryPhoto.NewsPhotoId));

        builder.Property(x => x.NewsId)
            .IsRequired()
            .ValueGeneratedNever()
            .HasGuidConversion()
            .HasColumnName(nameof(NewsPrimaryPhoto.NewsId).ToSnakeCase());

        builder.Property(x => x.NewsPhotoId)
            .IsRequired()
            .ValueGeneratedNever()
            .HasGuidConversion()
            .HasColumnName(nameof(NewsPrimaryPhoto.NewsPhotoId).ToSnakeCase());

        builder.HasOne(x => x.News)
            .WithOne(x => x.PrimaryPhoto)
            .HasForeignKey<NewsPrimaryPhoto>(x => x.NewsId);

        builder.HasOne(x => x.NewsPhoto)
            .WithOne(x => x.PrimaryPhoto)
            .HasForeignKey<NewsPrimaryPhoto>(x => x.NewsPhotoId);
    }
}