namespace Events.Domain.Entities;

public class Participant : Account
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateOnly BirthDate { get; set; }
    public string Email { get; set; } = null!;

    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = [];
}
