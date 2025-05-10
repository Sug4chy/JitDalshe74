using JitDalshe.Domain.Entities.Events;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class EventImageEntityTypeConfiguration : IEntityTypeConfiguration<EventImage>
{
    public void Configure(EntityTypeBuilder<EventImage> builder)
    {
        builder.ToTable(nameof(EventImage).ToSnakeCase());

        builder.HasId();
        builder.Property(x => x.Url)
            .IsRequired()
            .HasColumnName(nameof(EventImage.Url).ToSnakeCase());

        builder.Property(x => x.ContentType)
            .IsRequired()
            .HasColumnName(nameof(EventImage.ContentType).ToSnakeCase());

        builder.Property(x => x.EventId)
            .IsRequired()
            .HasGuidConversion()
            .HasColumnName(nameof(EventImage.EventId).ToSnakeCase());

        builder.HasAudits();

        builder.HasOne(x => x.Event)
            .WithOne(x => x.Image)
            .HasForeignKey<EventImage>(x => x.EventId);
    }
}