using Events.Application.Models.Account;

namespace Events.Application.Models.Participant;

public record ParticipantUpdateRequestModel(
    string? Username,
    string? Password,
    DateOnly? BirthDate,
    string? FirstName,
    string? LastName,
    string? Email
) : AccountUpdateRequestModel(Username, Password);
