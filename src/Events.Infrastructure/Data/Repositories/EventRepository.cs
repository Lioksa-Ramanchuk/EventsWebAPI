using Events.Domain.Entities;
using Events.Domain.Interfaces.Repositories;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class EventRepository(ApplicationDbContext dbContext, DeferredCommandManager commandManager)
    : BaseRepository(dbContext, commandManager),
        IEventRepository
{
    public void Add(Event evt)
    {
        ArgumentNullException.ThrowIfNull(evt, nameof(evt));

        base.Add(evt);
    }

    public async Task<Event?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Events.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public async Task<Event?> GetByIdAsTrackingAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext.Events.FindAsync([id], ct);
    }

    public async Task<Event?> GetByIdWithEventParticipantsAsync(Guid id, CancellationToken ct)
    {
        return await _dbContext
            .Events.AsNoTracking()
            .Include(e => e.EventParticipants)
            .FirstOrDefaultAsync(e => e.Id == id, ct);
    }

    public Task<Event?> GetByTitleAsync(string title, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        return _dbContext
            .Events.AsNoTracking()
            .FirstOrDefaultAsync(
                e => EF.Functions.Collate(e.Title, CaseInsensitiveCollation) == title,
                ct
            );
    }

    public async Task<List<Event>> GetUpcomingEventsHavingParticipantsAsync(
        (DateTime start, DateTime end) timeWindow,
        CancellationToken ct
    )
    {
        return await _dbContext
            .Events.AsNoTracking()
            .Where(e =>
                e.EventDate >= timeWindow.start
                && e.EventDate <= timeWindow.end
                && e.EventParticipants.Count != 0
            )
            .ToListAsync(ct);
    }

    public async Task<(List<Event> events, int totalCount)> GetPagedAllByFilterAsync(
        DateTime? startDateTime,
        DateTime? endDateTime,
        string? location,
        string? category,
        int offset,
        int limit,
        CancellationToken ct
    )
    {
        var eventsQuery = _dbContext.Events.AsNoTracking();

        if (startDateTime is not null)
        {
            eventsQuery = eventsQuery.Where(e => e.EventDate >= startDateTime.Value);
        }

        if (endDateTime is not null)
        {
            eventsQuery = eventsQuery.Where(e => e.EventDate <= endDateTime.Value);
        }

        if (string.IsNullOrEmpty(location))
        {
            eventsQuery = eventsQuery.Where(e =>
                EF.Functions.Collate(e.Location, CaseInsensitiveCollation) == location
            );
        }

        if (string.IsNullOrEmpty(category))
        {
            eventsQuery = eventsQuery.Where(e =>
                EF.Functions.Collate(e.Category, CaseInsensitiveCollation) == category
            );
        }

        var totalCount = await eventsQuery.CountAsync(ct);

        return (
            await eventsQuery.OrderBy(e => e.CreatedAt).Skip(offset).Take(limit).ToListAsync(ct),
            totalCount
        );
    }

    public void Remove(Event evt)
    {
        ArgumentNullException.ThrowIfNull(evt, nameof(evt));

        base.Remove(evt);
    }

    public async Task<bool> AnyWithTitleAsync(string title, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        return await _dbContext.Events.AnyAsync(
            e => EF.Functions.Collate(e.Title, CaseInsensitiveCollation) == title,
            ct
        );
    }
}
