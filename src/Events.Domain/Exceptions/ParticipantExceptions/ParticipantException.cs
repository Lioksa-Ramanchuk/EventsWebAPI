namespace Events.Domain.Exceptions.ParticipantExceptions;

public abstract class ParticipantException(string message) : EventsWebApiException(message) { }
