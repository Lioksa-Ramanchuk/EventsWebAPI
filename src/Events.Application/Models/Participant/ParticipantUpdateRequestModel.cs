using Events.Application.Models.Account;

namespace Events.Application.Models.Participant;

public class ParticipantUpdateRequestModel : AccountUpdateRequestModel
{
    public DateOnly? BirthDate { get; set; }
    public string? FirstName { get; set; } = null!;
    public string? LastName { get; set; } = null!;
    public string? Email { get; set; } = null!;
}
