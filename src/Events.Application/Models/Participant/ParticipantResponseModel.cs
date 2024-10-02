using Events.Application.Models.Account;

namespace Events.Application.Models.Participant;

public class ParticipantResponseModel : AccountResponseModel
{
    public ParticipantResponseModel()
        : base() { }

    public ParticipantResponseModel(
        Guid id,
        string username,
        string firstName,
        string lastName,
        DateOnly birthDate,
        string email,
        DateTime createdAt
    )
        : base(id, username, createdAt)
    {
        FirstName = firstName;
        LastName = lastName;
        BirthDate = birthDate;
        Email = email;
    }

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; } = null!;
}
