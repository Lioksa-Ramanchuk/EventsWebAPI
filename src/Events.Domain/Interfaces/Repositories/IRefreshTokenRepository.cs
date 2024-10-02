using Events.Domain.Entities;

namespace Events.Domain.Interfaces.Repositories;

public interface IRefreshTokenRepository
{
    void Add(RefreshToken refreshToken);
    Task<RefreshToken?> GetByTokenAsTrackingAsync(string token, CancellationToken ct = default);
    void RemoveAllExpired();
}
