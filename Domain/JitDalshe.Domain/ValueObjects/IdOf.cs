using CSharpFunctionalExtensions;

namespace JitDalshe.Domain.ValueObjects;

public readonly record struct IdOf<TEntity> : IComparable<IdOf<TEntity>> 
    where TEntity : Entity<IdOf<TEntity>>
{
    private readonly Guid _value;

    private IdOf(Guid value)
    {
        _value = value;
    }

    public static IdOf<TEntity> New() 
        => new(Guid.NewGuid());

    public static IdOf<TEntity> From(Guid value)
        => new(value);

    public static implicit operator Guid(IdOf<TEntity> value)
        => value._value;

    public int CompareTo(IdOf<TEntity> other)
    {
        return _value.CompareTo(other._value);
    }

    public override string ToString()
        => _value.ToString();
}