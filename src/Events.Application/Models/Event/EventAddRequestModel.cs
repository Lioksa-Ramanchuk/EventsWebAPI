namespace Events.Application.Models.Event;

public record EventAddRequestModel(
    string Title,
    string Description,
    DateTime EventDate,
    string Location,
    string Category,
    int MaxParticipantsCount
);
