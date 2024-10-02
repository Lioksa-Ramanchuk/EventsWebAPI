using Events.Domain.Entities;
using Events.Domain.Interfaces.Repositories;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class ParticipantRepository(
    ApplicationDbContext dbContext,
    DeferredCommandManager commandManager
) : BaseRepository(dbContext, commandManager), IParticipantRepository
{
    public void Add(Participant participant)
    {
        ArgumentNullException.ThrowIfNull(participant, nameof(participant));

        base.Add(participant);
    }

    public async Task<Participant?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext
            .Participants.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<Participant?> GetByIdAsTrackingAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Participants.FindAsync([id], ct);
    }

    public async Task<(List<Participant> participants, int totalCount)> GetPagedAllAsync(
        int offset,
        int limit,
        CancellationToken ct
    )
    {
        var participants = _dbContext.Participants.AsNoTracking();

        var totalCount = await participants.CountAsync(ct);

        return (
            await participants.OrderBy(p => p.CreatedAt).Skip(offset).Take(limit).ToListAsync(ct),
            totalCount
        );
    }
}
