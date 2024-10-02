namespace Events.Domain.Exceptions;

public class EventsWebApiException : Exception
{
    public EventsWebApiException() { }

    public EventsWebApiException(string message)
        : base(message) { }

    public EventsWebApiException(string message, Exception innerException)
        : base(message, innerException) { }
}
