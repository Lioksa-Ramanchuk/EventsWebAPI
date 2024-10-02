namespace Events.Domain.Entities;

public class Event : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = null!;
    public string Category { get; set; } = null!;
    public int MaxParticipantsCount { get; set; }
    public string? ImageFileName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<EventParticipant> EventParticipants { get; set; } = [];
}
