namespace Events.Application.Models.Participant;

public record ParticipantSignUpRequestModel(
    string Username,
    string Password,
    string FirstName,
    string LastName,
    DateOnly BirthDate,
    string Email
);
