using Events.Application.Models.Account;

namespace Events.Application.Models.Participant;

public class ParticipantResponseModel : AccountResponseModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; } = null!;
}
