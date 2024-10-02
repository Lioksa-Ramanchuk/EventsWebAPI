namespace Events.Domain.Exceptions.AuthExceptions.JwtExceptions;

public abstract class JwtException : AuthException
{
    public JwtException(string message)
        : base(message) { }

    public JwtException(string message, Exception innerException)
        : base(message, innerException) { }
}
