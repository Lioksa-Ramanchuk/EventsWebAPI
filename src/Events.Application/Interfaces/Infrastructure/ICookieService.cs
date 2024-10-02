namespace Events.Application.Interfaces.Infrastructure;

public interface ICookieService
{
    void ClearCookie(string key);
    string? GetCookie(string key);
    void SetCookie(string key, string value, int expireTimeInSeconds);
}
