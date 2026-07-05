using CSharpFunctionalExtensions;
using JitDalshe.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace JitDalshe.Infrastructure.Persistence.Converters;

public sealed class IdOfValueConverter<TEntity> : ValueConverter<IdOf<TEntity>, Guid>
    where TEntity : Entity<IdOf<TEntity>>
{
    public IdOfValueConverter()
        : base(
            id => id,
            value => IdOf<TEntity>.From(value))
    { }
}