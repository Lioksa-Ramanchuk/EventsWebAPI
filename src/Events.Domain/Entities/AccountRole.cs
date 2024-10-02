namespace Events.Domain.Entities;

public class AccountRole
{
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;

    public Guid RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;

    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}
