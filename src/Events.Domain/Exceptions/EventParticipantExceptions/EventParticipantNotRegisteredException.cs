using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.EventParticipantExceptions;

public class EventParticipantNotRegisteredException(Guid participantId, Guid eventId)
    : EventParticipantException(
        Smart.Format(
            ExceptionMessages.EventParticipantWithKeyNotRegistered,
            new { participantId, eventId }
        )
    ),
        IBadRequestException { }
