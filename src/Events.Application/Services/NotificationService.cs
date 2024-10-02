using AutoMapper;
using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Services;
using Events.Application.Models.Common;
using Events.Application.Models.Notification;
using Events.Domain.Entities;
using Events.Domain.Exceptions.AuthExceptions;
using Events.Domain.Exceptions.NotificationExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Microsoft.Extensions.Options;

namespace Events.Application.Services;

public class NotificationService(
    IDbUnitOfWork db,
    IMapper mapper,
    IOptions<AppSettings> appSettings
) : INotificationService
{
    private readonly ValidationSettings _validationSettings = appSettings.Value.ValidationSettings;

    public async Task<NotificationResponseModel> SendNotificationAsync(
        Guid accountId,
        NotificationSendRequestModel sendModel,
        CancellationToken ct
    )
    {
        if (await db.Accounts.GetByIdAsync(accountId, ct) is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        var notification = new Notification
        {
            AccountId = accountId,
            Message = sendModel.Message,
            IsRead = false,
            SentAt = DateTime.UtcNow,
        };

        db.Notifications.Add(notification);
        await db.SaveChangesAsync(ct);

        return mapper.Map<NotificationResponseModel>(notification);
    }

    public async Task<NotificationResponseModel> GetNotificationByIdAsModelAsync(
        Guid notificationId,
        CancellationToken ct
    )
    {
        var notification =
            await db.Notifications.GetByIdAsync(notificationId, ct)
            ?? throw new NotificationNotFoundException(notificationId);

        return mapper.Map<NotificationResponseModel>(notification);
    }

    public async Task<PagedResponseModel<NotificationResponseModel>> GetNotificationsAsModelsAsync(
        NotificationFilterRequestModel filterModel,
        Guid? accountId,
        CancellationToken ct
    )
    {
        if (accountId is not null && await db.Accounts.GetByIdAsync(accountId.Value, ct) is null)
        {
            throw new AccountNotFoundException(accountId.Value);
        }

        var offset = filterModel.Offset ?? 0;
        var limit = filterModel.Limit ?? _validationSettings.MaxFilterLimit;

        var (notificationsList, totalCount) = await db.Notifications.GetPagedAllByFilterAsync(
            accountId,
            offset,
            limit,
            ct
        );

        var notificationModels = mapper.Map<List<NotificationResponseModel>>(notificationsList);

        return new PagedResponseModel<NotificationResponseModel>
        {
            TotalCount = totalCount,
            Offset = offset,
            Limit = limit,
            Data = notificationModels,
        };
    }

    public async Task<NotificationResponseModel> MarkNotificationAsReadAsync(
        Guid notificationId,
        CancellationToken ct
    )
    {
        var notification =
            await db.Notifications.GetByIdAsTrackingAsync(notificationId, ct)
            ?? throw new NotificationNotFoundException(notificationId);

        notification.IsRead = true;

        await db.SaveChangesAsync(ct);

        return mapper.Map<NotificationResponseModel>(notification);
    }

    public async Task<int> MarkAccountNotificationsAsReadAsync(Guid accountId, CancellationToken ct)
    {
        if (await db.Accounts.GetByIdAsync(accountId, ct) is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        db.Notifications.UpdateAllAsReadByAccountId(accountId);
        var readCount = await db.SaveChangesAsync(ct);

        return readCount;
    }

    public async Task DeleteNotificationAsync(Guid notificationId, CancellationToken ct)
    {
        var notification =
            await db.Notifications.GetByIdAsTrackingAsync(notificationId, ct)
            ?? throw new NotificationNotFoundException(notificationId);

        db.Notifications.Remove(notification);
        await db.SaveChangesAsync(ct);
    }

    public async Task<int> DeleteAccountNotificationsAsync(Guid accountId, CancellationToken ct)
    {
        if (await db.Accounts.GetByIdAsync(accountId, ct) is null)
        {
            throw new AccountNotFoundException(accountId);
        }

        db.Notifications.RemoveAllByAccountId(accountId);
        var deletedCount = await db.SaveChangesAsync(ct);

        return deletedCount;
    }

    public async Task NotifyAccountsAsync(
        IEnumerable<Guid> accountIds,
        NotificationSendRequestModel sendModel,
        CancellationToken ct
    )
    {
        foreach (var accountId in accountIds)
        {
            var notification = mapper.Map<Notification>(sendModel);
            notification.AccountId = accountId;

            db.Notifications.Add(notification);
        }
        await db.SaveChangesAsync(ct);
    }
}
