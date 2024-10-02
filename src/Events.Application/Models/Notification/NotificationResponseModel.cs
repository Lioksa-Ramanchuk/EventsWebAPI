using Events.Application.Models.Common;

namespace Events.Application.Models.Notification;

public class NotificationResponseModel : BaseResponseModel
{
    public NotificationResponseModel()
        : base() { }

    public NotificationResponseModel(
        Guid id,
        string message,
        bool isRead,
        DateTime sentAt,
        Guid accountId
    )
        : base(id)
    {
        Message = message;
        IsRead = isRead;
        SentAt = sentAt;
        AccountId = accountId;
    }

    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; }
    public Guid AccountId { get; set; }
}
