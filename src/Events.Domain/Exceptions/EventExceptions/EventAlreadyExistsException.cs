using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.EventExceptions;

public class EventAlreadyExistsException(string message)
    : EventException(message),
        IConflictException { }
