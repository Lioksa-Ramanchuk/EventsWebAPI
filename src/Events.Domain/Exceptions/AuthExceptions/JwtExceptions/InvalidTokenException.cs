using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.AuthExceptions.JwtExceptions;

public class InvalidTokenException : JwtException, IBadRequestException
{
    public InvalidTokenException(string message)
        : base(message) { }

    public InvalidTokenException(string message, Exception innerException)
        : base(message, innerException) { }
}
