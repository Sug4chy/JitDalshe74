using CSharpFunctionalExtensions;
using JitDalshe.Domain.Abstractions;
using JitDalshe.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.Extensions;

public static class EntityTypeBuilderExtensions
{
    public static void HasAudits<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IAuditableEntity
    {
        builder.Property(x => x.CreatedAt)
            .ValueGeneratedNever()
            .HasColumnName(nameof(IAuditableEntity.CreatedAt).ToSnakeCase());

        builder.Property(x => x.UpdatedAt)
            .ValueGeneratedNever()
            .HasColumnName(nameof(IAuditableEntity.UpdatedAt).ToSnakeCase());
    }

    public static void HasId<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : Entity<IdOf<TEntity>>
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever()
            .IsRequired()
            .HasGuidConversion()
            .HasColumnName("id");
    }
}