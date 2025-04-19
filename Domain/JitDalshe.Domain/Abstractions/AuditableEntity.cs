using CSharpFunctionalExtensions;

namespace JitDalshe.Domain.Abstractions;

public abstract class AuditableEntity<TId> : Entity<TId>, IAuditableEntity
    where TId : IComparable<TId>
{
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; set; }
}