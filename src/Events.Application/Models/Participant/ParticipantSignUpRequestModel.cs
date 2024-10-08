namespace Events.Application.Models.Participant;

public class ParticipantSignUpRequestModel
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; } = null!;
}
