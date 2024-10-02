using Events.Infrastructure.Data.Context;

namespace Events.Infrastructure.Data.Commands;

public class DeferredAdd<TEntity>(TEntity entity) : IDeferredCommand
    where TEntity : class
{
    public async Task ExecuteAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        await dbContext.Set<TEntity>().AddAsync(entity, ct);
    }
}
