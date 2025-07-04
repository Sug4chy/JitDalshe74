using CSharpFunctionalExtensions;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static void HasAudits<TEntity>(this EntityTypeBuilder<TEntity> entity)
        where TEntity : class, IAuditableEntity
    {
        entity.Property(x => x.CreatedAt)
            .ValueGeneratedNever()
            .HasColumnName(nameof(IAuditableEntity.CreatedAt).ToSnakeCase());

        entity.Property(x => x.UpdatedAt)
            .ValueGeneratedNever()
            .HasColumnName(nameof(IAuditableEntity.UpdatedAt).ToSnakeCase());
    }

    public static void HasId<TEntity>(this EntityTypeBuilder<TEntity> entity)
        where TEntity : Entity<IdOf<TEntity>>
    {
        entity.HasKey(x => x.Id);

        entity.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired()
            .HasGuidConversion()
            .HasColumnName("id");
    }
}