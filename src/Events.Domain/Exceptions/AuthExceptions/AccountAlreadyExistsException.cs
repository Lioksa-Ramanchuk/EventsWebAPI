using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.AuthExceptions;

public class AccountAlreadyExistsException(string message)
    : AuthException(message),
        IConflictException { }
