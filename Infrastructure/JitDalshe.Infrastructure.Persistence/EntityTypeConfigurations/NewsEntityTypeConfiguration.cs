using JitDalshe.Domain.Entities.News;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class NewsEntityTypeConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.ToTable(nameof(News).ToSnakeCase());

        builder.HasId();

        builder.Property(x => x.ExtId)
            .IsRequired()
            .HasColumnName(nameof(News.ExtId).ToSnakeCase());
        builder.HasIndex(x => x.ExtId).IsUnique();

        builder.Property(x => x.Text)
            .IsRequired()
            .HasColumnName(nameof(News.Text).ToSnakeCase());

        builder.Property(x => x.PublicationDate)
            .HasColumnType("date")
            .IsRequired()
            .HasColumnName(nameof(News.PublicationDate).ToSnakeCase());

        builder.Property(x => x.PostUrl)
            .IsRequired()
            .HasColumnName(nameof(News.PostUrl).ToSnakeCase());

        builder.Property(x => x.IsDisplaying)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName(nameof(News.IsDisplaying).ToSnakeCase());

        builder.HasAudits();

        builder.HasMany(x => x.Images)
            .WithOne(x => x.News)
            .HasForeignKey(x => x.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.PrimaryImage)
            .WithOne(x => x.News)
            .HasForeignKey<NewsPrimaryImage>(x => x.NewsId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}