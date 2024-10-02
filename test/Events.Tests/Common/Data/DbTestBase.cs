using System.Data;
using Events.Infrastructure.Data.UnitOfWork;
using Events.Tests.Common.Fixtures;

namespace Events.Tests.Common.Data;

public abstract class DbTestBase(DbFixture dbFixture) : IAsyncLifetime
{
    protected DbUnitOfWork _dbUnitOfWork = null!;

    public virtual async Task InitializeAsync()
    {
        _dbUnitOfWork = new DbUnitOfWork(dbFixture.DbContext);
        await _dbUnitOfWork.BeginTransactionAsync(IsolationLevel.ReadCommitted, default);
    }

    public virtual async Task DisposeAsync()
    {
        await _dbUnitOfWork.RollbackAsync(default);
    }
}
