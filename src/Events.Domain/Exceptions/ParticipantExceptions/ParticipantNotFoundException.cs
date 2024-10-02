using Events.Domain.Exceptions.StatusCodeExceptionInterfaces;
using Events.Domain.Resources;
using SmartFormat;

namespace Events.Domain.Exceptions.ParticipantExceptions;

public class ParticipantNotFoundException(Guid participantId)
    : ParticipantException(
        Smart.Format(ExceptionMessages.ParticipantWithIdNotFound, new { participantId })
    ),
        INotFoundException { }
