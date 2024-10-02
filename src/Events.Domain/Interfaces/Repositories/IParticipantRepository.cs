using Events.Domain.Entities;

namespace Events.Domain.Interfaces.Repositories;

public interface IParticipantRepository
{
    void Add(Participant participant);
    Task<Participant?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Participant?> GetByIdAsTrackingAsync(Guid id, CancellationToken ct = default);
    Task<(List<Participant> participants, int totalCount)> GetPagedAllAsync(
        int offset,
        int limit,
        CancellationToken ct = default
    );
}
