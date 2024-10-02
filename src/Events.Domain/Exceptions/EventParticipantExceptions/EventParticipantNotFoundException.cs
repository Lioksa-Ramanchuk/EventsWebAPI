using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.EventParticipantExceptions;

public class EventParticipantNotFoundException(Guid eventId, Guid participantId)
    : EventParticipantException(
        Smart.Format(
            ExceptionMessages.EventParticipantWithKeyNotFound,
            new { eventId, participantId }
        )
    ),
        INotFoundException { }
