using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.AuthExceptions.JwtExceptions;

public class MissingTokenException(string message) : JwtException(message), IBadRequestException { }
