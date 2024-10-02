using Events.Infrastructure.Data.Context;

namespace Events.Infrastructure.Data.Commands;

public class DeferredRemove<TEntity>(TEntity entity) : IDeferredCommand
    where TEntity : class
{
    public async Task ExecuteAsync(ApplicationDbContext dbContext, CancellationToken _)
    {
        dbContext.Set<TEntity>().Remove(entity);
        await Task.CompletedTask;
    }
}
