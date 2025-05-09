using JitDalshe.Domain.Entities.Events;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class EventPhotoEntityTypeConfiguration : IEntityTypeConfiguration<EventPhoto>
{
    public void Configure(EntityTypeBuilder<EventPhoto> builder)
    {
        builder.ToTable(nameof(EventPhoto).ToSnakeCase());

        builder.HasId();
        builder.Property(x => x.Url)
            .IsRequired()
            .HasColumnName(nameof(EventPhoto.Url).ToSnakeCase());

        builder.Property(x => x.EventId)
            .IsRequired()
            .HasGuidConversion()
            .HasColumnName(nameof(EventPhoto.EventId).ToSnakeCase());

        builder.HasAudits();

        builder.HasOne(x => x.Event)
            .WithOne(x => x.Photo)
            .HasForeignKey<EventPhoto>(x => x.EventId);
    }
}