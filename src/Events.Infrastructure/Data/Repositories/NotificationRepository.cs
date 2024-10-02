using Events.Domain.Entities;
using Events.Domain.Interfaces.Repositories;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class NotificationRepository(
    ApplicationDbContext dbContext,
    DeferredCommandManager commandManager
) : BaseRepository(dbContext, commandManager), INotificationRepository
{
    public void Add(Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        base.Add(notification);
    }

    public async Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext
            .Notifications.AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == id, ct);
    }

    public async Task<Notification?> GetByIdAsTrackingAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Notifications.FindAsync([id], ct);
    }

    public async Task<(List<Notification> notifications, int totalCount)> GetPagedAllByFilterAsync(
        Guid? accountId,
        int offset,
        int limit,
        CancellationToken ct
    )
    {
        var notificationsQuery = _dbContext.Notifications.AsNoTracking();

        if (accountId is not null)
        {
            notificationsQuery = notificationsQuery.Where(n => n.AccountId == accountId.Value);
        }

        var totalCount = await notificationsQuery.CountAsync(ct);

        return (
            await notificationsQuery
                .OrderBy(ep => ep.SentAt)
                .Skip(offset)
                .Take(limit)
                .ToListAsync(ct),
            totalCount
        );
    }

    public void UpdateAllAsReadByAccountId(Guid accountId)
    {
        UpdateRange(
            db => db.Notifications.Where(n => n.AccountId == accountId),
            set => set.SetProperty(n => n.IsRead, true)
        );
    }

    public void Remove(Notification notification)
    {
        ArgumentNullException.ThrowIfNull(notification, nameof(notification));

        base.Remove(notification);
    }

    public void RemoveAllByAccountId(Guid accountId)
    {
        base.RemoveRange(db => db.Notifications.Where(n => n.AccountId == accountId));
    }
}
