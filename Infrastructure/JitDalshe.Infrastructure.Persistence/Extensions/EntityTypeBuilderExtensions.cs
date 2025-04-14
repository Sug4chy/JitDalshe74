using JitDalshe.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static EntityTypeBuilder<TEntity> HasAudits<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IAuditableEntity
    {
        builder.Property(x => x.CreatedAt)
            .ValueGeneratedOnAdd()
            .HasColumnName(nameof(IAuditableEntity.CreatedAt).ToSnakeCase());

        builder.Property(x => x.UpdatedAt)
            .ValueGeneratedOnUpdate()
            .HasColumnName(nameof(IAuditableEntity.UpdatedAt).ToSnakeCase());

        return builder;
    }
}