using JitDalshe.Domain.Entities.Reviews;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class ReviewEntityTypeConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> entity)
    {
        entity.ToTable(nameof(Review).ToSnakeCase());

        entity.HasId();

        entity.Property(x => x.ReviewerName)
            .IsRequired()
            .HasColumnName(nameof(Review.ReviewerName).ToSnakeCase());

        entity.Property(x => x.ReviewerAge)
            .IsRequired()
            .HasColumnName(nameof(Review.ReviewerAge).ToSnakeCase());

        entity.Property(x => x.ReviewerStatus)
            .IsRequired()
            .HasColumnName(nameof(Review.ReviewerStatus).ToSnakeCase());

        entity.Property(x => x.Text)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName(nameof(Review.Text).ToSnakeCase());

        entity.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(ReviewStatus.New)
            .HasColumnName(nameof(Review.Status).ToSnakeCase());
        
        entity.HasAudits();
    }
}