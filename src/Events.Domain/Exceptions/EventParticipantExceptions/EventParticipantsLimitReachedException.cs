using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.EventParticipantExceptions;

public class EventMaxParticipantsReachedException(Guid eventId, int eventMaxParticipantsCount)
    : EventParticipantException(
        Smart.Format(
            ExceptionMessages.EventWithIdMaxParticipantsWithValueReached,
            new { eventId, eventMaxParticipantsCount }
        )
    ),
        IBadRequestException { }
