using Events.Infrastructure.Data.Context;

namespace Events.Infrastructure.Data.Commands;

public class DeferredUpdate<TEntity>(TEntity entity) : IDeferredCommand
    where TEntity : class
{
    public async Task ExecuteAsync(ApplicationDbContext dbContext, CancellationToken _)
    {
        dbContext.Set<TEntity>().Update(entity);
        await Task.CompletedTask;
    }
}
