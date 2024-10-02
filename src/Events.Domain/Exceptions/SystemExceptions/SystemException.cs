using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.SystemExceptions;

public class SystemException(string message)
    : EventsWebApiException(message),
        IInternalServerErrorException { }
