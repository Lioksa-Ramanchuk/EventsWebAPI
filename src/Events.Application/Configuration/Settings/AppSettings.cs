namespace Events.Application.Configuration.Settings;

public class AppSettings
{
    public AuthorizationSettings AuthorizationSettings { get; set; } = null!;
    public CacheSettings CacheSettings { get; set; } = null!;
    public ConnectionStrings ConnectionStrings { get; set; } = null!;
    public CorsSettings CorsSettings { get; set; } = null!;
    public CryptoSettings CryptoSettings { get; set; } = null!;
    public JwtSettings JwtSettings { get; set; } = null!;
    public MediaSettings MediaSettings { get; set; } = null!;
    public NotificationSettings NotificationSettings { get; set; } = null!;
    public ValidationSettings ValidationSettings { get; set; } = null!;
}
