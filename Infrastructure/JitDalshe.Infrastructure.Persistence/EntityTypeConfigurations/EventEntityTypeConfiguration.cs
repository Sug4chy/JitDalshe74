using JitDalshe.Domain.Entities.Events;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.ToTable(nameof(Event).ToSnakeCase());

        builder.HasId();

        builder.Property(x => x.Title)
            .IsRequired()
            .HasColumnName(nameof(Event.Title).ToSnakeCase());

        builder.Property(x => x.Description)
            .HasColumnName(nameof(Event.Description).ToSnakeCase());

        builder.Property(x => x.Date)
            .IsRequired()
            .HasColumnType("date")
            .HasColumnName(nameof(Event.Date).ToSnakeCase());

        builder.HasAudits();

        builder.HasOne(x => x.Image)
            .WithOne(x => x.Event)
            .HasForeignKey<EventImage>(x => x.EventId);
    }
}