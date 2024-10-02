using System.Data;
using Events.Domain.Interfaces.Repositories;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Domain.Resources;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Events.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Events.Infrastructure.Data.UnitOfWork;

public class DbUnitOfWork : IDbUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _dbContext;
    private readonly Lazy<IRoleRepository> _roles;
    private readonly Lazy<IAccountRepository> _accounts;
    private readonly Lazy<IAccountRoleRepository> _accountRoles;
    private readonly Lazy<IRefreshTokenRepository> _refreshTokens;
    private readonly Lazy<IParticipantRepository> _participants;
    private readonly Lazy<IEventRepository> _events;
    private readonly Lazy<IEventParticipantRepository> _eventParticipants;
    private readonly Lazy<INotificationRepository> _notifications;

    private readonly DeferredCommandManager _commandManager;
    private IDbContextTransaction? _transaction;
    private bool _disposedValue;

    public DbUnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _commandManager = new();
        _roles = new(() => new RoleRepository(dbContext, _commandManager));
        _accounts = new(() => new AccountRepository(dbContext, _commandManager));
        _accountRoles = new(() => new AccountRoleRepository(dbContext, _commandManager));
        _refreshTokens = new(() => new RefreshTokenRepository(dbContext, _commandManager));
        _participants = new(() => new ParticipantRepository(dbContext, _commandManager));
        _events = new(() => new EventRepository(dbContext, _commandManager));
        _eventParticipants = new(() => new EventParticipantRepository(dbContext, _commandManager));
        _notifications = new(() => new NotificationRepository(dbContext, _commandManager));
    }

    public IRoleRepository Roles => _roles.Value;
    public IAccountRepository Accounts => _accounts.Value;
    public IAccountRoleRepository AccountRoles => _accountRoles.Value;
    public IRefreshTokenRepository RefreshTokens => _refreshTokens.Value;
    public IParticipantRepository Participants => _participants.Value;
    public IEventRepository Events => _events.Value;
    public IEventParticipantRepository EventParticipants => _eventParticipants.Value;
    public INotificationRepository Notifications => _notifications.Value;

    public async Task<int> SaveChangesAsync(CancellationToken ct)
    {
        bool IsInOuterTransaction = _transaction is not null;

        try
        {
            if (!IsInOuterTransaction)
            {
                await BeginTransactionAsync(IsolationLevel.ReadCommitted, ct);
            }

            await _commandManager.ExecuteCommandsAsync(_dbContext, ct);

            var result = await _dbContext.SaveChangesAsync(ct);

            if (!IsInOuterTransaction && _transaction is not null)
            {
                await _transaction.CommitAsync(ct);
                _transaction = null;
            }

            return result;
        }
        catch
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync(ct);
                _transaction = null;
            }
            throw;
        }
    }

    public async Task BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken ct)
    {
        if (_transaction is not null)
        {
            throw new SystemException(ExceptionMessages.TransactionAlreadyInUse);
        }
        _transaction = await _dbContext.Database.BeginTransactionAsync(isolationLevel, ct);
    }

    public async Task CommitAsync(CancellationToken ct)
    {
        if (_transaction is not null)
        {
            await _transaction.CommitAsync(ct);
            _transaction = null;
        }
    }

    public async Task RollbackAsync(CancellationToken ct)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(ct);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction = null;
            }
            _disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
