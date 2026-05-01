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

        entity.Property(x => x.ShortDescription)
            .IsRequired()
            .HasColumnName(nameof(Event.ShortDescription).ToSnakeCase());

        entity.Property(x => x.FullText)
            .IsRequired()
            .HasColumnType("text")
            .HasColumnName(nameof(Event.FullText).ToSnakeCase());
        
        entity.Property(x => x.Date)
            .HasColumnType("date")
            .HasColumnName(nameof(Event.Date).ToSnakeCase());
        
        entity.Property(x => x.Time)
            .HasColumnName(nameof(Event.Time).ToSnakeCase());
        
        entity.Property(x => x.Location)
            .HasColumnName(nameof(Event.Location).ToSnakeCase());
        
        entity.Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(EventStatus.NotPublished)
            .HasColumnName(nameof(Event.Status).ToSnakeCase());
        
        entity.HasAudits();

        entity.HasOne(x => x.Image)
            .WithOne(x => x.Event)
            .HasForeignKey<EventImage>(x => x.EventId);
    }
}