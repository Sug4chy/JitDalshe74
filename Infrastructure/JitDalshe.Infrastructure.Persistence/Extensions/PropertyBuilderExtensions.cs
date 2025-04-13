using CSharpFunctionalExtensions;
using JitDalse.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JitDalshe.Infrastructure.Persistence.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<IdOf<TEntity>> HasGuidConversion<TEntity>(this PropertyBuilder<IdOf<TEntity>> builder)
        where TEntity : Entity<IdOf<TEntity>>
        => builder.HasConversion<Guid>(
            id => id,
            value => IdOf<TEntity>.From(value)
        );
}