using Events.Application.Configuration.Settings;
using Events.Application.Interfaces.Infrastructure;
using Events.Domain.Entities;
using Events.Domain.Exceptions.AuthExceptions;
using Events.Domain.Exceptions.AuthExceptions.JwtExceptions;
using Events.Domain.Interfaces.UnitOfWork;
using Events.Domain.Resources;
using Microsoft.Extensions.Options;
using SmartFormat;

namespace Events.Infrastructure.Services;

public class JwtManagementHelperService(
    IJwtService jwtService,
    ICookieService cookieService,
    IDbUnitOfWork db,
    IOptions<AppSettings> appSettings
) : IJwtManagementHelperService
{
    private readonly JwtSettings _jwtSettings = appSettings.Value.JwtSettings;

    public async Task SetTokensAsync(Account accountWithRoles, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(accountWithRoles, nameof(accountWithRoles));

        var accessToken = jwtService.GenerateAccessToken(accountWithRoles);
        var refreshToken = jwtService.GenerateRefreshToken();
        await StoreRefreshTokenAsync(accountWithRoles.Id, refreshToken, ct);
        SetAccessTokenCookie(accessToken);
        SetRefreshTokenCookie(refreshToken);
    }

    public void ClearTokens()
    {
        ClearAccessTokenCookie();
        ClearRefreshTokenCookie();
    }

    public async Task RefreshTokensAsync(CancellationToken ct)
    {
        var accessToken =
            GetAccessTokenCookie()
            ?? throw new MissingTokenException(ExceptionMessages.AccessTokenMissing);
        var refreshToken =
            GetRefreshTokenCookie()
            ?? throw new MissingTokenException(ExceptionMessages.RefreshTokenMissing);

        var accountId = jwtService.GetAccountIdFromToken(accessToken);
        var accountWithRoles =
            await db.Accounts.GetByIdWithRolesAsync(accountId, ct)
            ?? throw new AccountNotFoundException(accountId);

        var storedRefreshToken = await db.RefreshTokens.GetByTokenAsTrackingAsync(refreshToken, ct);
        if (storedRefreshToken is null || storedRefreshToken.AccountId != accountId)
        {
            throw new TokenNotFoundException(
                Smart.Format(
                    ExceptionMessages.RefreshTokenStoredForAccountWithIdNotFound,
                    new { accountId }
                )
            );
        }
        else if (!storedRefreshToken.IsActive)
        {
            throw new ExpiredTokenException(
                Smart.Format(
                    ExceptionMessages.RefreshTokenStoredForAccountWithIdExpired,
                    new { accountId }
                )
            );
        }

        var newAccessToken = jwtService.GenerateAccessToken(accountWithRoles);
        var newRefreshToken = jwtService.GenerateRefreshToken();

        storedRefreshToken.Token = newRefreshToken;
        storedRefreshToken.CreatedAt = DateTime.UtcNow;
        storedRefreshToken.ExpiresAt = storedRefreshToken.CreatedAt.AddSeconds(
            _jwtSettings.RefreshTokenExpiresInSeconds
        );
        await db.SaveChangesAsync(ct);

        SetAccessTokenCookie(newAccessToken);
        SetRefreshTokenCookie(newRefreshToken);
    }

    public async Task StoreRefreshTokenAsync(
        Guid accountId,
        string refreshToken,
        CancellationToken ct
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

        var now = DateTime.UtcNow;
        var expires = now.AddSeconds(_jwtSettings.RefreshTokenExpiresInSeconds);

        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            CreatedAt = now,
            ExpiresAt = expires,
            AccountId = accountId,
        };

        db.RefreshTokens.Add(refreshTokenEntity);
        await db.SaveChangesAsync(ct);
    }

    public async Task CleanupExpiredStoredTokensAsync(CancellationToken ct)
    {
        db.RefreshTokens.RemoveAllExpired();
        await db.SaveChangesAsync(ct);
    }

    public void SetAccessTokenCookie(string accessToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(accessToken, nameof(accessToken));

        cookieService.SetCookie(
            _jwtSettings.AccessTokenCookieName,
            accessToken,
            _jwtSettings.AccessTokenCookieExpiresInSeconds
        );
    }

    public void SetRefreshTokenCookie(string refreshToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(refreshToken, nameof(refreshToken));

        cookieService.SetCookie(
            _jwtSettings.RefreshTokenCookieName,
            refreshToken,
            _jwtSettings.RefreshTokenCookieExpiresInSeconds
        );
    }

    public string? GetAccessTokenCookie()
    {
        return cookieService.GetCookie(_jwtSettings.AccessTokenCookieName);
    }

    public string? GetRefreshTokenCookie()
    {
        return cookieService.GetCookie(_jwtSettings.RefreshTokenCookieName);
    }

    public void ClearAccessTokenCookie()
    {
        cookieService.ClearCookie(_jwtSettings.AccessTokenCookieName);
    }

    public void ClearRefreshTokenCookie()
    {
        cookieService.ClearCookie(_jwtSettings.RefreshTokenCookieName);
    }
}
