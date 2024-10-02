namespace Events.Domain.Entities;

public class Account : BaseEntity
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<AccountRole> AccountRoles { get; set; } = [];
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public virtual ICollection<Notification> Notifications { get; set; } = [];
}
