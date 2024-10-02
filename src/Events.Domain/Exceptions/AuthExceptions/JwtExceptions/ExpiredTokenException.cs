using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.AuthExceptions.JwtExceptions;

public class ExpiredTokenException(string message)
    : InvalidTokenException(message),
        IBadRequestException { }
