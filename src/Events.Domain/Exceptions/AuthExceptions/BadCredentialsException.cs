using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.AuthExceptions;

public class BadCredentialsException(string message)
    : AuthException(message),
        IUnauthorizedException { }
