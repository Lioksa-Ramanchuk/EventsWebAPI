using Events.Application.Models.Common;
using Events.Application.Models.Notification;

namespace Events.Application.Interfaces.Services
{
    public interface INotificationService
    {
        Task DeleteNotificationAsync(Guid notificationId, CancellationToken ct = default);
        Task<int> DeleteAccountNotificationsAsync(Guid accountId, CancellationToken ct = default);
        Task<NotificationResponseModel> GetNotificationByIdAsModelAsync(
            Guid notificationId,
            CancellationToken ct = default
        );
        Task<PagedResponseModel<NotificationResponseModel>> GetNotificationsAsModelsAsync(
            NotificationFilterRequestModel filterModel,
            Guid? accountId = null,
            CancellationToken ct = default
        );
        Task<int> MarkAccountNotificationsAsReadAsync(
            Guid accountId,
            CancellationToken ct = default
        );
        Task<NotificationResponseModel> MarkNotificationAsReadAsync(
            Guid notificationId,
            CancellationToken ct = default
        );
        Task NotifyAccountsAsync(
            IEnumerable<Guid> accountIds,
            NotificationSendRequestModel sendModel,
            CancellationToken ct = default
        );
        Task<NotificationResponseModel> SendNotificationAsync(
            Guid accountId,
            NotificationSendRequestModel sendModel,
            CancellationToken ct = default
        );
    }
}
