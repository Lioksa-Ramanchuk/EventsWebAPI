namespace Events.Domain.Exceptions.MediaExceptions;

public abstract class MediaException : EventsWebApiException
{
    public MediaException(string message)
        : base(message) { }

    public MediaException(string message, Exception innerException)
        : base(message, innerException) { }
}
