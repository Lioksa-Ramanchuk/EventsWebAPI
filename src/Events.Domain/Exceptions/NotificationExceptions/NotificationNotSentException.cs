using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.NotificationExceptions;

public class NotificationNotSentException(Guid notificationId, Guid accountId)
    : NotificationException(
        Smart.Format(
            ExceptionMessages.NotificationWithIdNotSentToAccountWithId,
            new { notificationId, accountId }
        )
    ),
        INotFoundException { }
