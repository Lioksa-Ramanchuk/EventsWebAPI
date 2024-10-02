namespace Events.Application.Configuration.Settings;

public class JwtSettings
{
    public string Issuer { get; set; } = null!;
    public string[] Audiences { get; set; } = null!;
    public string SecretKey { get; set; } = null!;

    public int AccessTokenExpiresInSeconds { get; set; }
    public string AccessTokenCookieName { get; set; } = null!;
    public int AccessTokenCookieExpiresInSeconds { get; set; }

    public int RefreshTokenExpiresInSeconds { get; set; }
    public string RefreshTokenCookieName { get; set; } = null!;
    public int RefreshTokenCookieExpiresInSeconds { get; set; }

    public int ExpiredRefreshTokensCleanupIntervalInSeconds { get; set; }
}
