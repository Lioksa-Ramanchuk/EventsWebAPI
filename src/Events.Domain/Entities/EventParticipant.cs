namespace Events.Domain.Entities;

public class EventParticipant
{
    public Guid EventId { get; set; }
    public virtual Event Event { get; set; } = null!;

    public Guid ParticipantId { get; set; }
    public virtual Participant Participant { get; set; } = null!;

    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

    public bool IsNotifiedToday { get; set; } = false;
}
