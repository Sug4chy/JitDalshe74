using JitDalshe.Domain.Entities.Events;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class EventImageEntityTypeConfiguration : IEntityTypeConfiguration<EventImage>
{
    public void Configure(EntityTypeBuilder<EventImage> entity)
    {
        entity.ToTable(nameof(EventImage).ToSnakeCase());

        entity.HasId();
        entity.Property(x => x.Url)
            .IsRequired()
            .HasColumnName(nameof(EventImage.Url).ToSnakeCase());

        entity.Property(x => x.ContentType)
            .IsRequired()
            .HasColumnName(nameof(EventImage.ContentType).ToSnakeCase());

        entity.Property(x => x.EventId)
            .IsRequired()
            .HasGuidConversion()
            .HasColumnName(nameof(EventImage.EventId).ToSnakeCase());

        entity.HasAudits();

        entity.HasOne(x => x.Event)
            .WithOne(x => x.Image)
            .HasForeignKey<EventImage>(x => x.EventId);
    }
}