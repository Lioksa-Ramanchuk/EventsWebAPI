using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Commands;

public class DeferredRemoveRange<TEntity>(Func<ApplicationDbContext, IQueryable<TEntity>> queryFunc)
    : IDeferredCommand
    where TEntity : class
{
    public async Task ExecuteAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        await queryFunc(dbContext).ExecuteDeleteAsync(ct);
    }
}
