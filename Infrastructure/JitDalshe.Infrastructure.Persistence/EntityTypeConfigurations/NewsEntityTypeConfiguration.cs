using JitDalshe.Domain.Entities;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class NewsEntityTypeConfiguration : IEntityTypeConfiguration<News>
{
    public void Configure(EntityTypeBuilder<News> builder)
    {
        builder.ToTable(nameof(News).ToSnakeCase());

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .IsRequired()
            .ValueGeneratedNever()
            .HasGuidConversion()
            .HasColumnName(nameof(News.Id).ToSnakeCase());

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

        builder.HasAudits();

        builder.HasMany(x => x.Photos)
            .WithOne(x => x.News)
            .HasForeignKey(x => x.NewsId);

        builder.HasOne(x => x.PrimaryPhoto)
            .WithOne(x => x.News)
            .HasForeignKey<NewsPrimaryPhoto>(x => x.NewsId);

    }
}