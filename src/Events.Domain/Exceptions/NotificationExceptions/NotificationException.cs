namespace Events.Domain.Exceptions.NotificationExceptions;

public abstract class NotificationException(string message) : EventsWebApiException(message) { }
