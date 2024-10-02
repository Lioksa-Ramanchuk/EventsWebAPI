namespace Events.Domain.Exceptions.AuthExceptions;

public abstract class AuthException : EventsWebApiException
{
    public AuthException() { }

    public AuthException(string message)
        : base(message) { }

    public AuthException(string message, Exception innerException)
        : base(message, innerException) { }
}
