using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.EventParticipantExceptions;

public class EventParticipantAlreadyRegisteredException(Guid participantId, Guid eventId)
    : EventParticipantException(
        Smart.Format(
            ExceptionMessages.EventParticipantWithKeyAlreadyRegistered,
            new { participantId, eventId }
        )
    ),
        IConflictException { }
