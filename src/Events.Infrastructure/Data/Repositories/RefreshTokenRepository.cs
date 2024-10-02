using Events.Domain.Entities;
using Events.Domain.Interfaces.Repositories;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;

namespace Events.Infrastructure.Data.Repositories;

public class RefreshTokenRepository(
    ApplicationDbContext dbContext,
    DeferredCommandManager commandManager
) : BaseRepository(dbContext, commandManager), IRefreshTokenRepository
{
    public void Add(RefreshToken refreshToken)
    {
        ArgumentNullException.ThrowIfNull(refreshToken, nameof(refreshToken));

        base.Add(refreshToken);
    }

    public async Task<RefreshToken?> GetByTokenAsTrackingAsync(string token, CancellationToken ct)
    {
        return await _dbContext.RefreshTokens.FindAsync([token], ct);
    }

    public void RemoveAllExpired()
    {
        base.RemoveRange(db => db.RefreshTokens.Where(rt => rt.IsExpired));
    }
}
