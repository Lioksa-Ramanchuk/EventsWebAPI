using System.Linq.Expressions;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Events.Infrastructure.Data.Commands;

public class DeferredUpdateRange<TEntity>(
    Func<ApplicationDbContext, IQueryable<TEntity>> queryFunc,
    Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> updateExpression
) : IDeferredCommand
    where TEntity : class
{
    public async Task ExecuteAsync(ApplicationDbContext dbContext, CancellationToken ct)
    {
        await queryFunc(dbContext).ExecuteUpdateAsync(updateExpression, ct);
    }
}
