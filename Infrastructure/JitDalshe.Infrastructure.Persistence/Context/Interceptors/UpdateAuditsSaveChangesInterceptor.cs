using JitDalshe.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace JitDalshe.Infrastructure.Persistence.Context.Interceptors;

public sealed class UpdateAuditsSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return new ValueTask<InterceptionResult<int>>(result);
        }

        UpdateAudits(dbContext);

        return new ValueTask<InterceptionResult<int>>(result);
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return result;
        }

        UpdateAudits(dbContext);

        return result;
    }

    private static void UpdateAudits(DbContext dbContext)
    {
        var entries = dbContext.ChangeTracker.Entries<IAuditableEntity>();
        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                    break;
                case EntityState.Modified:
                    entry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    break;
                case EntityState.Added:
                    entry.Property(e => e.CreatedAt).CurrentValue = DateTime.UtcNow;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}