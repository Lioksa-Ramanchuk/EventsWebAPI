namespace Events.Domain.Exceptions.EventExceptions;

public abstract class EventException : EventsWebApiException
{
    public EventException()
        : base() { }

    public EventException(string message)
        : base(message) { }
}
