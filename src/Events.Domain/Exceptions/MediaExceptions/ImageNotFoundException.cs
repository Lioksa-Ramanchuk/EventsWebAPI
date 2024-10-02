using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.MediaExceptions;

public class ImageNotFoundException(string message)
    : MediaException(message),
        INotFoundException { }
