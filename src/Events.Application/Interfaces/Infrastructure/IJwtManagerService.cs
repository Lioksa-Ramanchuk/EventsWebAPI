using Events.Domain.Entities;

namespace Events.Application.Interfaces.Infrastructure;

public interface IJwtManagementHelperService
{
    Task CleanupExpiredStoredTokensAsync(CancellationToken ct = default);
    void ClearAccessTokenCookie();
    void ClearRefreshTokenCookie();
    void ClearTokens();
    string? GetAccessTokenCookie();
    string? GetRefreshTokenCookie();
    Task RefreshTokensAsync(CancellationToken ct = default);
    void SetAccessTokenCookie(string accessToken);
    void SetRefreshTokenCookie(string refreshToken);
    Task SetTokensAsync(Account accountWithRoles, CancellationToken ct = default);
    Task StoreRefreshTokenAsync(
        Guid accountId,
        string refreshToken,
        CancellationToken ct = default
    );
}
