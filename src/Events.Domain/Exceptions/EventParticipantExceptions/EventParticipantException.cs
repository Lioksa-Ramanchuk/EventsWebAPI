namespace Events.Domain.Exceptions.EventParticipantExceptions;

public abstract class EventParticipantException(string message) : EventsWebApiException(message) { }
