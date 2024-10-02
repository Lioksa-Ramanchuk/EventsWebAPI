using System.Linq.Expressions;

namespace Events.Domain.Entities;

public class RefreshToken
{
    public string Token { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }

    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;

    public static Expression<Func<RefreshToken, bool>> IsExpiredExpr =>
        rt => DateTime.UtcNow >= rt.ExpiresAt;
    public static Expression<Func<RefreshToken, bool>> IsActiveExpr =>
        rt => DateTime.UtcNow < rt.ExpiresAt;

    public bool IsExpired => IsExpiredExpr.Compile()(this);
    public bool IsActive => IsActiveExpr.Compile()(this);
}
