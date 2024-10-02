using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.NotificationExceptions;

public class NotificationNotFoundException(Guid notificationId)
    : NotificationException(
        Smart.Format(ExceptionMessages.NotificationWithIdNotFound, new { notificationId })
    ),
        INotFoundException { }
