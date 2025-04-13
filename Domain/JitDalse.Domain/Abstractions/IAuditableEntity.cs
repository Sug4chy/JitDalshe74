namespace JitDalse.Domain.Abstractions;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; }
    public DateTime UpdatedAt { get; set; }
}