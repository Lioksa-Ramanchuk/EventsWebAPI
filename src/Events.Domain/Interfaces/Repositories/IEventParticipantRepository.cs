using Events.Domain.Entities;

namespace Events.Domain.Interfaces.Repositories;

public interface IEventParticipantRepository
{
    void Add(EventParticipant eventParticipant);
    Task<EventParticipant?> GetByKeyAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct = default
    );
    Task<EventParticipant?> GetByKeyAsTrackingAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct = default
    );
    Task<EventParticipant?> GetByKeyWithParticipantAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct = default
    );
    Task<EventParticipant?> GetByKeyWithEventAsync(
        Guid eventId,
        Guid participantId,
        CancellationToken ct = default
    );
    Task<List<EventParticipant>> GetAllUnnotifiedByEventIdAsTrackingAsync(
        Guid eventId,
        CancellationToken ct = default
    );
    Task<List<Guid>> GetAllParticipantIdsByEventIdAsync(
        Guid eventId,
        CancellationToken ct = default
    );
    Task<(
        List<EventParticipant> eventParticipants,
        int totalCount
    )> GetPagedAllWithParticipantByEventIdAsync(
        Guid eventId,
        int offset,
        int limit,
        CancellationToken ct = default
    );
    Task<(
        List<EventParticipant> participantEvents,
        int totalCount
    )> GetPagedAllWithEventByParticipantIdAsync(
        Guid participantId,
        int offset,
        int limit,
        CancellationToken ct = default
    );
    void Remove(EventParticipant eventParticipant);
}
