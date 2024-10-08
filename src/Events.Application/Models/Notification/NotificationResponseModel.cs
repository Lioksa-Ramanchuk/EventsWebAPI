using Events.Application.Models.Common;

namespace Events.Application.Models.Notification;

public class NotificationResponseModel : BaseResponseModel
{
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; }
    public Guid AccountId { get; set; }
}
