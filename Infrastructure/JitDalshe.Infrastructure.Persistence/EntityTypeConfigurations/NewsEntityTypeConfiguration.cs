using JitDalshe.Domain.Entities.News;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class NewsEntityTypeConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> entity)
    {
        entity.ToTable(nameof(News).ToSnakeCase());

        entity.Property(x => x.ExtId)
            .IsRequired()
            .HasColumnName(nameof(News.ExtId).ToSnakeCase());
        entity.HasIndex(x => x.ExtId).IsUnique();

        entity.Property(x => x.Text)
            .IsRequired()
            .HasColumnName(nameof(News.Text).ToSnakeCase());

        entity.Property(x => x.PublicationDate)
            .HasColumnType("date")
            .IsRequired()
            .HasColumnName(nameof(News.PublicationDate).ToSnakeCase());

        entity.Property(x => x.PostUrl)
            .IsRequired()
            .HasColumnName(nameof(News.PostUrl).ToSnakeCase());

        entity.Property(x => x.IsDisplaying)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName(nameof(News.IsDisplaying).ToSnakeCase());

        entity.HasMany(x => x.Images)
            .WithOne(x => x.News)
            .HasForeignKey(x => x.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(x => x.PrimaryImage)
            .WithOne(x => x.News)
            .HasForeignKey<NewsPrimaryImage>(x => x.NewsId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}