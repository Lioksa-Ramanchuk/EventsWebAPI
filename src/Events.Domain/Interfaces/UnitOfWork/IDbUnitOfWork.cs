using System.Data;
using Events.Domain.Interfaces.Repositories;

namespace Events.Domain.Interfaces.UnitOfWork;

public interface IDbUnitOfWork
{
    IRoleRepository Roles { get; }
    IAccountRepository Accounts { get; }
    IAccountRoleRepository AccountRoles { get; }
    IRefreshTokenRepository RefreshTokens { get; }
    IParticipantRepository Participants { get; }
    IEventRepository Events { get; }
    IEventParticipantRepository EventParticipants { get; }
    INotificationRepository Notifications { get; }

    Task<int> SaveChangesAsync(CancellationToken ct = default);

    Task BeginTransactionAsync(
        IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
        CancellationToken ct = default
    );
    Task CommitAsync(CancellationToken ct = default);
    Task RollbackAsync(CancellationToken ct = default);
}
