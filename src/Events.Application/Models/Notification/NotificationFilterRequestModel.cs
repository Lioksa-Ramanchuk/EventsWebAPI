using Events.Application.Models.Common;

namespace Events.Application.Models.Notification;

public record NotificationFilterRequestModel(int? Offset, int? Limit)
    : BaseFilterRequestModel(Offset, Limit);
