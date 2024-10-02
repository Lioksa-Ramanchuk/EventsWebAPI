using Events.Domain.Entities;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventRepository
{
    void Add(Event evt);
    Task<Event?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Event?> GetByIdAsTrackingAsync(Guid id, CancellationToken ct = default);
    Task<Event?> GetByIdWithEventParticipantsAsync(Guid id, CancellationToken ct = default);
    Task<Event?> GetByTitleAsync(string title, CancellationToken ct = default);
    Task<List<Event>> GetUpcomingEventsHavingParticipantsAsync(
        (DateTime start, DateTime end) timeWindow,
        CancellationToken ct = default
    );
    Task<(List<Event> events, int totalCount)> GetPagedAllByFilterAsync(
        DateTime? startDateTime,
        DateTime? endDateTime,
        string? location,
        string? category,
        int offset,
        int limit,
        CancellationToken ct
    );
    Task<bool> AnyWithTitleAsync(string title, CancellationToken ct = default);
    void Remove(Event evt);
}
