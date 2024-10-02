using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;

namespace Events.Domain.Exceptions.EventExceptions;

public class NoEventImageException : EventException, INoContentException { }
