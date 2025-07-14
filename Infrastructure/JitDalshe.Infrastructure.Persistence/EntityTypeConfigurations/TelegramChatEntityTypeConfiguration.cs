using JitDalshe.Application.Entities;
using JitDalshe.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.EntityTypeConfigurations;

public sealed class TelegramChatEntityTypeConfiguration : IEntityTypeConfiguration<TelegramChat>
{
    public void Configure(EntityTypeBuilder<TelegramChat> entity)
    {
        entity.ToTable(nameof(TelegramChat).ToSnakeCase());

        entity.HasId();

        entity.Property(x => x.ExtId)
            .IsRequired()
            .HasColumnName(nameof(TelegramChat.ExtId).ToSnakeCase());

        entity.HasAudits();
    }
}