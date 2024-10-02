using Events.Domain.Entities;
using Events.Domain.Interfaces.Repositories;
using Events.Infrastructure.Data.Commands;
using Events.Infrastructure.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Events.Infrastructure.Data.Repositories;

public class AccountRoleRepository(
    ApplicationDbContext dbContext,
    DeferredCommandManager commandManager
) : BaseRepository(dbContext, commandManager), IAccountRoleRepository
{
    public void Add(AccountRole accountRole)
    {
        ArgumentNullException.ThrowIfNull(accountRole, nameof(accountRole));

        base.Add(accountRole);
    }

    public async Task<AccountRole?> GetByKeyAsync(Guid accountId, Guid roleId, CancellationToken ct)
    {
        return await _dbContext
            .AccountRoles.AsNoTracking()
            .FirstOrDefaultAsync(ar => ar.AccountId == accountId && ar.RoleId == roleId, ct);
    }

    public async Task<AccountRole?> GetByKeyAsTrackingAsync(
        Guid accountId,
        Guid roleId,
        CancellationToken ct
    )
    {
        return await _dbContext.AccountRoles.FindAsync([accountId, roleId], ct);
    }

    public async Task<AccountRole?> GetByKeyWithRoleAsync(
        Guid accountId,
        Guid roleId,
        CancellationToken ct
    )
    {
        return await _dbContext
            .AccountRoles.AsNoTracking()
            .Include(ar => ar.Role)
            .SingleOrDefaultAsync(ar => ar.AccountId == accountId && ar.RoleId == roleId, ct);
    }

    public async Task<(
        List<AccountRole> accountRoles,
        int totalCount
    )> GetPagedAllWithRoleByAccountIdAsync(
        Guid accountId,
        int offset,
        int limit,
        CancellationToken ct
    )
    {
        var accountRoles = _dbContext
            .AccountRoles.AsNoTracking()
            .Where(ar => ar.AccountId == accountId);

        var totalCount = await accountRoles.CountAsync(ct);

        return (
            await accountRoles
                .OrderBy(ar => ar.AssignedAt)
                .Skip(offset)
                .Take(limit)
                .Include(ar => ar.Role)
                .ToListAsync(ct),
            totalCount
        );
    }

    public void Remove(AccountRole accountRole)
    {
        ArgumentNullException.ThrowIfNull(accountRole, nameof(accountRole));

        base.Remove(accountRole);
    }

    public async Task<bool> AnyAccountWithRoleByTitle(string roleTitle, CancellationToken ct)
    {
        ArgumentException.ThrowIfNullOrEmpty(roleTitle, nameof(roleTitle));

        return await _dbContext.AccountRoles.AnyAsync(
            ar => EF.Functions.Collate(ar.Role.Title, CaseInsensitiveCollation) == roleTitle,
            ct
        );
    }

    public async Task<bool> AnyOtherAccountWithRoleTitleAsync(
        Guid accountId,
        string roleTitle,
        CancellationToken ct
    )
    {
        ArgumentException.ThrowIfNullOrEmpty(roleTitle, nameof(roleTitle));

        return await _dbContext.AccountRoles.AnyAsync(
            ar =>
                ar.AccountId != accountId
                && EF.Functions.Collate(ar.Role.Title, CaseInsensitiveCollation) == roleTitle,
            ct
        );
    }
}
