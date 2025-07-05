using JitDalshe.Domain.Entities.Events;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> entity)
    {
        entity.ToTable(nameof(Event).ToSnakeCase());

        entity.HasId();

        entity.Property(x => x.Title)
            .IsRequired()
            .HasColumnName(nameof(Event.Title).ToSnakeCase());

        entity.Property(x => x.Description)
            .HasColumnName(nameof(Event.Description).ToSnakeCase());

        entity.Property(x => x.Date)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(nameof(Event.Date).ToSnakeCase());

        entity.Property(x => x.IsDisplaying)
            .IsRequired()
            .HasDefaultValue(false)
            .HasColumnName(nameof(Event.IsDisplaying).ToSnakeCase());

        entity.HasAudits();

        entity.HasOne(x => x.Image)
            .WithOne(x => x.Event)
            .HasForeignKey<EventImage>(x => x.EventId);
    }
}