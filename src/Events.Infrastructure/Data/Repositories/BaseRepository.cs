using System.Linq.Expressions;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore.Query;

namespace Events.Infrastructure.Data.Repositories;

public abstract class BaseRepository(
    ApplicationDbContext dbContext,
    DeferredCommandManager commandManager
)
{
    protected const string CaseInsensitiveCollation = "Latin1_General_CI_AS";
    private readonly DeferredCommandManager _commandManager = commandManager;
    protected ApplicationDbContext _dbContext = dbContext;

    protected void Add<TEntity>(TEntity entity)
        where TEntity : class
    {
        var command = new DeferredAdd<TEntity>(entity);
        _commandManager.AddCommand(command);
    }

    protected void AddRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class
    {
        var command = new DeferredAddRange<TEntity>(entities);
        _commandManager.AddCommand(command);
    }

    protected void Update<TEntity>(TEntity entity)
        where TEntity : class
    {
        var command = new DeferredUpdate<TEntity>(entity);
        _commandManager.AddCommand(command);
    }

    protected void UpdateRange<TEntity>(
        Func<ApplicationDbContext, IQueryable<TEntity>> queryFunc,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> updateExpression
    )
        where TEntity : class
    {
        var command = new DeferredUpdateRange<TEntity>(queryFunc, updateExpression);
        _commandManager.AddCommand(command);
    }

    protected void Remove<TEntity>(TEntity entity)
        where TEntity : class
    {
        var command = new DeferredRemove<TEntity>(entity);
        _commandManager.AddCommand(command);
    }

    protected void RemoveRange<TEntity>(Func<ApplicationDbContext, IQueryable<TEntity>> queryFunc)
        where TEntity : class
    {
        var command = new DeferredRemoveRange<TEntity>(queryFunc);
        _commandManager.AddCommand(command);
    }
}
