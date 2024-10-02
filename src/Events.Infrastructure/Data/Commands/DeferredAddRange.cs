using Events.Infrastructure.Data.Context;

namespace Events.Infrastructure.Data.Commands;

public class DeferredAddRange<TEntity>(IEnumerable<TEntity> entities) : IDeferredCommand
    where TEntity : class
{
    public async Task ExecuteAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        await dbContext.Set<TEntity>().AddRangeAsync(entities, ct);
    }
}
