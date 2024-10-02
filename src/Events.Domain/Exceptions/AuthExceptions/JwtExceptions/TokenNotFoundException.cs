using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.AuthExceptions.JwtExceptions;

public class TokenNotFoundException(string message) : JwtException(message), INotFoundException { }
