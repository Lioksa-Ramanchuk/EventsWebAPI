using Events.Domain.Entities;

namespace Events.Domain.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Account?> GetByIdAsTrackingAsync(Guid id, CancellationToken ct = default);
    Task<Account?> GetByIdWithRolesAsync(Guid id, CancellationToken ct = default);
    Task<Account?> GetByUsernameWithRolesAsync(string username, CancellationToken ct = default);
    Task<(List<Account> accounts, int totalCount)> GetPagedAllAsync(
        int offset,
        int limit,
        CancellationToken ct = default
    );
    Task<bool> AnyWithUsernameAsync(string username, CancellationToken ct = default);
    void Remove(Account account);
}
