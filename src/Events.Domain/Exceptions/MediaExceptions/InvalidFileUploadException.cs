using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.MediaExceptions;

public class InvalidFileUploadException(string message)
    : MediaException(message),
        IBadRequestException { }
