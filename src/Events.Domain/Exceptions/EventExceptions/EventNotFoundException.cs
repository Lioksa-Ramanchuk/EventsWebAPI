using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.EventExceptions;

public class EventNotFoundException : EventException, INotFoundException
{
    public EventNotFoundException(Guid eventId)
        : base(Smart.Format(ExceptionMessages.EventWithIdNotFound, new { eventId })) { }

    public EventNotFoundException(string message)
        : base(message) { }
}
