using Events.Application.Interfaces.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Events.Infrastructure.Services;

public class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    public void SetCookie(string key, string value, int expireTimeInSeconds)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));
        ArgumentNullException.ThrowIfNull(value, nameof(value));

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddSeconds(expireTimeInSeconds),
        };

        httpContextAccessor.HttpContext?.Response.Cookies.Append(key, value, cookieOptions);
    }

    public string? GetCookie(string key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        return httpContextAccessor.HttpContext?.Request.Cookies[key];
    }

    public void ClearCookie(string key)
    {
        ArgumentNullException.ThrowIfNull(key, nameof(key));

        httpContextAccessor.HttpContext?.Response.Cookies.Delete(key);
    }
}
