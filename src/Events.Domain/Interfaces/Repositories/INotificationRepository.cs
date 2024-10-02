using Events.Domain.Entities;

namespace Events.Domain.Interfaces.Repositories;

public interface INotificationRepository
{
    void Add(Notification notification);
    Task<Notification?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Notification?> GetByIdAsTrackingAsync(Guid id, CancellationToken ct = default);
    Task<(List<Notification> notifications, int totalCount)> GetPagedAllByFilterAsync(
        Guid? accountId,
        int offset,
        int limit,
        CancellationToken ct = default
    );
    void UpdateAllAsReadByAccountId(Guid accountId);
    void Remove(Notification notification);
    void RemoveAllByAccountId(Guid accountId);
}
