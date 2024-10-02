using Events.Domain.Entities;
using Events.Domain.Interfaces.Repositories;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class EventParticipantRepository(
    ApplicationDbContext dbContext,
    DeferredCommandManager commandManager
) : BaseRepository(dbContext, commandManager), IEventParticipantRepository
{
    public void Add(EventParticipant eventParticipant)
    {
        ArgumentNullException.ThrowIfNull(eventParticipant, nameof(eventParticipant));

        base.Add(eventParticipant);
    }

    public async Task<EventParticipant?> GetByKeyAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct
    )
    {
        return await _dbContext
            .EventParticipants.AsNoTracking()
            .FirstOrDefaultAsync(
                ep => ep.EventId == eventId && ep.ParticipantId == participantId,
                ct
            );
    }

    public async Task<EventParticipant?> GetByKeyAsTrackingAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct
    )
    {
        return await _dbContext.EventParticipants.FindAsync([eventId, participantId], ct);
    }

    public async Task<EventParticipant?> GetByKeyWithParticipantAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct
    )
    {
        return await _dbContext
            .EventParticipants.AsNoTracking()
            .Include(ep => ep.Participant)
            .FirstOrDefaultAsync(
                ep => ep.EventId == eventId && ep.ParticipantId == participantId,
                ct
            );
    }

    public async Task<EventParticipant?> GetByKeyWithEventAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct
    )
    {
        return await _dbContext
            .EventParticipants.AsNoTracking()
            .Include(ep => ep.Event)
            .FirstOrDefaultAsync(
                ep => ep.EventId == eventId && ep.ParticipantId == participantId,
                ct
            );
    }

    public async Task<List<EventParticipant>> GetAllUnnotifiedByEventIdAsTrackingAsync(
        Guid eventId,
        CancellationToken ct
    )
    {
        return await _dbContext
            .EventParticipants.Where(ep => ep.EventId == eventId && !ep.IsNotifiedToday)
            .ToListAsync(ct);
    }

    public async Task<List<Guid>> GetAllParticipantIdsByEventIdAsync(
        Guid eventId,
        CancellationToken ct
    )
    {
        return await _dbContext
            .EventParticipants.Where(ep => ep.EventId == eventId)
            .Select(ep => ep.ParticipantId)
            .ToListAsync(ct);
    }

    public async Task<(
        List<EventParticipant> eventParticipants,
        int totalCount
    )> GetPagedAllWithParticipantByEventIdAsync(
        Guid eventId,
        int offset,
        int limit,
        CancellationToken ct
    )
    {
        var eventParticipants = _dbContext
            .EventParticipants.AsNoTracking()
            .Where(ep => ep.EventId == eventId);

        var totalCount = await eventParticipants.CountAsync(ct);

        return (
            await eventParticipants
                .OrderBy(ep => ep.RegistrationDate)
                .Skip(offset)
                .Take(limit)
                .Include(ep => ep.Participant)
                .ToListAsync(ct),
            totalCount
        );
    }

    public async Task<(
        List<EventParticipant> participantEvents,
        int totalCount
    )> GetPagedAllWithEventByParticipantIdAsync(
        Guid participantId,
        int offset,
        int limit,
        CancellationToken ct
    )
    {
        var eventParticipants = _dbContext
            .EventParticipants.AsNoTracking()
            .Where(ep => ep.ParticipantId == participantId);

        var totalCount = await eventParticipants.CountAsync(ct);

        return (
            await eventParticipants
                .OrderBy(ep => ep.RegistrationDate)
                .Skip(offset)
                .Take(limit)
                .Include(ep => ep.Event)
                .ToListAsync(ct),
            totalCount
        );
    }

    public void Remove(EventParticipant eventParticipant)
    {
        ArgumentNullException.ThrowIfNull(eventParticipant, nameof(eventParticipant));

        base.Remove(eventParticipant);
    }
}
