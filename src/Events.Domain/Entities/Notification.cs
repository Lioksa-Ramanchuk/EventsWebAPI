namespace Events.Domain.Entities;

public class Notification : BaseEntity
{
    public string Message { get; set; } = null!;
    public bool IsRead { get; set; } = false;
    public DateTime SentAt { get; set; }

    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;
}
