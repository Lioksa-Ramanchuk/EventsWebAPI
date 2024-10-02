using Events.Domain.Entities;

namespace Events.Domain.Interfaces.Repositories;

public interface IAccountRoleRepository
{
    void Add(AccountRole accountRole);
    Task<AccountRole?> GetByKeyAsync(Guid accountId, Guid roleId, CancellationToken ct = default);
    Task<AccountRole?> GetByKeyAsTrackingAsync(
        Guid accountId,
        Guid roleId,
        CancellationToken ct = default
    );
    Task<AccountRole?> GetByKeyWithRoleAsync(
        Guid accountId,
        Guid roleId,
        CancellationToken ct = default
    );
    Task<(List<AccountRole> accountRoles, int totalCount)> GetPagedAllWithRoleByAccountIdAsync(
        Guid accountId,
        int offset,
        int limit,
        CancellationToken ct
    );
    void Remove(AccountRole accountRole);
    Task<bool> AnyAccountWithRoleByTitle(string roleTitle, CancellationToken ct = default);
    Task<bool> AnyOtherAccountWithRoleTitleAsync(
        Guid accountId,
        string roleTitle,
        CancellationToken ct = default
    );
}
