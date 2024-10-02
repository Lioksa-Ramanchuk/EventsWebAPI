using Events.Domain.Entities;

namespace Events.Domain.Interfaces.Repositories;

public interface IRoleRepository
{
    void Add(Role role);
    Task<Role?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Role?> GetByTitleAsync(string title, CancellationToken ct = default);
}
